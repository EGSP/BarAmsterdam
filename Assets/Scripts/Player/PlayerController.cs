using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Player.PlayerStates;
using Player.PlayerCursors;
using Interiors;
using Items.MonoItems;

// TODO: Добавить метод обновления камеры. 
// TODO: Сделать вызов обновления камеры при любом перемещении. 
namespace Player.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        /// <summary>
        /// Маска объектов, с которыми будет обработка столкновений
        /// </summary>
        public LayerMask CollisionMask { get => collisionMask; }
        [SerializeField] private LayerMask collisionMask;

        /// <summary>
        /// Время передвижения персонажа в секундах
        /// </summary>
        public float MoveTime { get => moveTime; }
        [Header("Movement")] [Range(0, 10)] [SerializeField] private float moveTime;

        /// <summary>
        /// Шаг передвижения
        /// </summary>
        public float MoveStep { get => moveStep; }
        [SerializeField] private float moveStep;

        /// <summary>
        /// Множитель велечины шага передвижения по вертикали
        /// </summary>
        public float VerticalStepModifier { get => verticalStepModifier; }
        [Range(0,1)][SerializeField] private float verticalStepModifier;

        /// <summary>
        /// Местоположение руки, куда будут складываться предметы
        /// </summary>
        public Transform HandTransform { get => handTransform; }
        [SerializeField] private Transform handTransform;
        /// <summary>
        /// Настольный курсор для предметов
        /// </summary>
        public TableTopCursor TableCursor { get => tableCursor; }
        [Header("Additional components")] [SerializeField] private TableTopCursor tableCursor;

        /// <summary>
        /// Аниматор используемый контроллером
        /// </summary>
        public Animator Animator { get => animator; private set => animator = value; }
        [SerializeField] private Animator animator;

        /// <summary>
        /// Компонент используемый для отрисовки спрайтов 
        /// </summary>
        public SpriteRenderer SpriteRenderer { get => spriteRenderer; private set => spriteRenderer = value; }
        [SerializeField] private SpriteRenderer spriteRenderer;

        [SerializeField] private CameraFollow cameraFollow;

        /// <summary>
        /// Инвертированное время используемое для передвижения
        /// </summary>
        private float InversedMoveTime { get => 1 / MoveTime; }

        /// <summary>
        /// Ориентация игрока (направление взгляда)
        /// </summary>
        public Vector3 Orientation { get => orientation; private set => orientation = value.normalized; }
        private Vector3 orientation = Vector3.right;

        /// <summary>
        /// Ориентация игрока учитываяющая модификатор шага
        /// </summary>
        public Vector3 ModifiedOrientation
        {
            get
            {
                var orient = Orientation;
                orient.y *= verticalStepModifier;
                return orient;
            }
        }

        /// <summary>
        /// Перемещается ли в данный момент персонаж
        /// </summary>
        public bool IsMoving { get; private set; }
        
        private IEnumerator movementRoutine;
        private Vector3 movementEndPosition = Vector3.zero;

        /// <summary>
        /// Текущее поведение персонажа
        /// </summary>
        private PlayerState CurrentPlayerState;

        /// <summary>
        /// Кешируем данные обновления, чтобы не пересоздавать каждый кадр
        /// </summary>
        private UpdateData updateData;
        
        
        private void Awake()
        {
            if (Animator == null)
                Animator = GetComponentInChildren<Animator>();

            if (SpriteRenderer == null)
                SpriteRenderer = GetComponentInChildren<SpriteRenderer>();

            if (cameraFollow == null)
            {
                Debug.Log("Камера игрока не установлена в PlayerController");
            }
            else
            {
                cameraFollow.Target = transform;
            }
                
            if (TableCursor == null)
                throw new System.Exception("TableCursor is null in PlayerController.cs. Set TableCursor component on PlayerController gameobject");

            Orientation = Vector3.right;

            updateData = new UpdateData();
            SetState(new BaseState(this));
        }

        // Update is called once per frame
        void Update()
        {
            updateData.DeltaTime = Time.deltaTime;
            CurrentPlayerState = CurrentPlayerState.UpdateState(updateData);
            // Debug.Log(CurrentPlayerState);
        }

        /// <summary>
        /// Движение персонажа в зависимости от нажатых клавиш. Проверки на границы значений клавиш нет.
        /// </summary>
        /// <param name="horizontalInput">Клавиши горизонтального ввода от -1 до 1</param>
        /// <param name="verticalInput">Клавиши вертикального ввода от -1 до 1</param>
        public void Move(int horizontalInput, int verticalInput)
        {
            var horizontalDir = horizontalInput * transform.right * MoveStep;
            var verticalDir = verticalInput * transform.up * MoveStep * VerticalStepModifier;

            Vector3 newPosition = transform.position;

            int horObstacleExist = CheckObstacleByLinecast(transform.position + horizontalDir) == true ? 1 : 0;
            int verObstacleExist = CheckObstacleByLinecast(transform.position + verticalDir) == true ? 1 : 0;

            
            // Если не можем никуда пойти (true)
            if (horObstacleExist == 1 && verObstacleExist == 1)
            {
                // Смена ориентации
                ChangeOrientation(horizontalInput, verticalInput);
                return;
            }

            
            // Если можем идти по диагонали. Нет препятствий по бокам
            if (horObstacleExist == 0 && verObstacleExist == 0 && (Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput)) == 2)
            {
                bool diaObstacleExist = CheckObstacleByLinecast(transform.position + horizontalDir + verticalDir);

                // Если в точке по диагонали нет препятствий
                if (diaObstacleExist == false)
                {
                    //// Смена ориентации
                    //ChangeOrientation(horizontalInput, verticalInput);
                    ChangeOrientation(horizontalInput, verticalInput);

                    
                    newPosition += horizontalDir;
                    newPosition += verticalDir;

                    movementRoutine = Movement(newPosition);
                    StartCoroutine(movementRoutine);

                    return;
                }
                else
                {
                    // Никуда не идем, даже если свободны стороны по вертикали и горизонтали
                    return;
                }
            }

            // Учитываем препятствия
            horizontalInput *= (1 - horObstacleExist);
            verticalInput *= (1 - verObstacleExist);

            // Смена ориентации
            ChangeOrientation(horizontalInput, 0);

            // Одно из направлений нулевое, но это не страшно
            newPosition += horizontalDir * (1 - horObstacleExist);
            newPosition += verticalDir * (1 - verObstacleExist);

            // Сокращаем время поиска нужной корутины
            // При перезначении старая корутина все еще может существовать, пока не доделает свою работу 
            movementRoutine = Movement(newPosition);
            StartCoroutine(movementRoutine);
        }

        public void MoveWithoutCollision(int horizontalInput, int verticalInput)
        {
            LayerMask saveCollision = collisionMask;
            collisionMask = ~1;
            Move(horizontalInput, verticalInput);
            collisionMask = saveCollision;
        }

        /// <summary>
        /// Смена ориентации, направления взгляда, игрока. Проверки на ноль есть.
        /// </summary>
        /// <param name="horizontal">Направление по горизонтали</param>
        /// <param name="vertical">Направление по вертикали</param>
        public void ChangeOrientation(int horizontal,int vertical)
        {
            // Если только одно из направлений действительно
            if ((Mathf.Abs(horizontal) + Mathf.Abs(vertical)) == 1)
            {
                Orientation = new Vector3(horizontal, vertical);
            }
            else
            {
                // Здесь оба могут быть нулевыми
                if (horizontal != 0)
                    Orientation = new Vector3(horizontal, Orientation.y);

                if (vertical != 0)
                    Orientation = new Vector3(Orientation.x, vertical);
            }
        }

        /// <summary>
        /// Корутина, передвигающая персонажа в новую позицию
        /// </summary>
        /// <param name="newPosition">Новая позиция</param>
        /// <returns></returns>
        public IEnumerator Movement(Vector3 newPosition)
        {
            var sqrMagnitude = (transform.position - newPosition).sqrMagnitude;

            IsMoving = true;
            movementEndPosition = newPosition;

            
            // Пока расстояние больше очень малого значения близкого к нулю
            while (sqrMagnitude > float.Epsilon)
            {
                transform.position = Vector3.MoveTowards(transform.position, newPosition,
                 InversedMoveTime * Time.deltaTime);

                sqrMagnitude = (transform.position - newPosition).sqrMagnitude;
                
                // Вручную обновляем камеру
                ManualUpdateCamera();
                
                // Ждем обновления кадра
                yield return null;
            }

            IsMoving = false;
        }

        /// <summary>
        /// Останавливает движение и моментально телепортирует игрока в конечную точку
        /// </summary>
        public void StopMovement()
        {
            StopCoroutine(movementRoutine);

            transform.position = movementEndPosition;
        }

        /// <summary>
        /// Проигрывание анимации по названию
        /// </summary>
        public void PlayAnimation(string animationName)
        {
            if (Animator != null)
            {
                Animator.Play(animationName);
            }
        }

        /// <summary>
        /// Проигрывание анимации по хешу. Animator.StringToHash(), чтобы получить хеш
        /// </summary>
        public void PlayAnimationByHash(int hash)
        {
            if (Animator != null)
            {
                Animator.Play(hash);
            }
        }

        /// <summary>
        /// Вызывает метод обновления камеры
        /// </summary>
        public void ManualUpdateCamera()
        {
            if(cameraFollow!=null)
                cameraFollow.MoveToTarget();
        }









        /// <summary>
        /// Проверка препятствий с помощью луча в конечную позицию
        /// </summary>
        /// <param name="endPosition">Конечная позиция</param>
        /// <returns></returns>
        public bool CheckObstacleByLinecast(Vector3 endPosition)
        {
            var hit = Physics2D.Linecast(transform.position, endPosition, CollisionMask);

            if (hit.collider != null)
                return true;

            return false;
        }

        /// <summary>
        /// Получение компонента с помощью луча. Возвращает null, если не был найден искомый компонент
        /// </summary>
        /// <typeparam name="T">Искомый компонент</typeparam>
        /// <param name="endPosition">Конечная позиция луча</param>
        /// <returns></returns>
        public T GetComponentByLinecast<T>(Vector3 endPosition) where T:class
        {
            var hit = Physics2D.Linecast(transform.position, endPosition, CollisionMask);

            if (hit.collider != null)
            {
                // Ищем компонент на объекте
                var component = hit.collider.gameObject.GetComponent<T>();

                if (component != null)
                    return component;

                return null;
            }

            return null;
        }

        /// <summary>
        /// Устанавливает новое состояние если оно не равно null
        /// </summary>
        /// <param name="behaviourState">Новое состояние</param>
        public void SetState(PlayerState playerState)
        {
            if (playerState != null)
            {
                if (CurrentPlayerState != null)
                {
                    // Освобождаем ресурсы прошлого состояния
                    CurrentPlayerState.Dispose();
                }

                // Присваиваем новое состояние
                CurrentPlayerState = playerState;
            }
        }
        
        // TODO: Переписать на более эффективную систему после реализации нескольких предметов
        public string TakeItem(Interior interior)
        {
            var item = interior.gameObject.transform.GetChild(0);
            item.parent = transform;
            return item.name;
        }

        public string PutItem(Interior interior)
        {
            var item = transform.GetChild(0);
            item.parent = interior.gameObject.transform;
            return "Nothing";
        }

        /// <summary>
        /// Устанавливает родительский объект предмету в руке
        /// </summary>
        public void PlaceItemToHand(MonoItem monoItem)
        {
            if (HandTransform == null)
            {
                Debug.Log("У персонажа не установлена рука");
                monoItem.transform.position = transform.position;
                monoItem.transform.SetParent(transform,true);
            }
            else
            {
                monoItem.transform.position = HandTransform.position;
                monoItem.transform.SetParent(HandTransform, true);
            }
        }

        private void OnDrawGizmosSelected()
        {
            // Отрисовка величины шага
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position - transform.right * MoveStep, transform.position + transform.right * MoveStep);
            Gizmos.DrawLine(
                transform.position - transform.up * MoveStep * VerticalStepModifier,
                transform.position + transform.up * MoveStep * VerticalStepModifier);

            // Направление взгляда
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + Orientation);
        }
    }
}