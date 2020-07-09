using System;
using System.Collections.Generic;
using System.Linq;
using Bots;
using Bots.Behaviours;
using Bots.Factory;
using Gasanov.Exceptions;
using Gasanov.Extensions;
using Gasanov.SpeedUtils;
using Gasanov.SpeedUtils.RandomUtilities;
using Gasanov.SpeedUtils.Time;
using Interiors;
using TMPro;
using UnityEngine;

namespace World
{
    [RequireComponent(typeof(FoodInfo))]
    public class Bar : MonoBehaviour
    {
        public static Bar Instance;
        
        [Header("Настройки времени")]
        [SerializeField][Range(0,24)] private int startHour;
        [SerializeField][Range(0,60)] private int startMinute;

        [SerializeField][Range(0,24)] private int endHour;
        [SerializeField][Range(0,60)] private int endMinute;
        [Range(0,10)] public float timeScale;

        [Space(10)] [Header("Настройки сложности")]
        [SerializeField] private CurveHolder curveHolder;

        [SerializeField][Range(0,100)] private float minChance;
        [SerializeField][Range(0,100)] private float maxChance;
        /// <summary>
        /// Задержка срабатывания проверки шансов
        /// </summary>
        [SerializeField] private float prokeDelay;
        private float prokeDelayCurrent;
        
        [Space(10)] [Header("Настройки объектов")] 
        [SerializeField] private Transform spawnPoint;
        
        /// <summary>
        /// Информация по еде
        /// </summary>
        public FoodInfo FoodInformation { get; private set; }
        
        /// <summary>
        /// Все стулья бара
        /// </summary>
        public List<Chair> Chairs { get; private set; }
        private RandomList<Chair> freeChairs;
        
        /// <summary>
        /// Есть ли в баре хоть один свободный стул
        /// </summary>
        public bool FreeChairAvailable => !freeChairs.IsEmpty;
        
        /// <summary>
        /// Таймер дня
        /// </summary>
        private HourTimer hourTimer;


        private List<CustomerBehaviour> customers = new List<CustomerBehaviour>();

        private event Action OnProkeEvent = delegate {  };

        private void Awake()
        {
            if(Instance != null)
                throw new SingletonException<Bar>(this);
            
            Instance = this;
            
            // ЕДА
            FoodInformation = GetComponent<FoodInfo>();
            if(FoodInformation == null)
                throw new NullReferenceException();
            
            // БОТЫ
            CustomersFactory.LoadCustomersPrefabs();
            OnProkeEvent += SpawnRandomCustomer;
            
            // СТУЛЬЯ
            SetupChairs();

            prokeDelayCurrent = prokeDelay;
        }
        
        private void Start()
        {
            hourTimer = new HourTimer(startHour,startMinute,endHour,endMinute);
            
            var text = UtilsClass.CreateWorldText("time",Color.white);
            
            ActionUpdater.Create((delta) =>
            {
                UpdateText(text);
                
            }, hourTimer.TargetMinutes);
            
            TestFunction();
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            
            // Обновляем время
            var timeOpacity = UpdateTime(deltaTime);

            var difficulty = curveHolder.GetCurveValue(timeOpacity);
            
            // Обрабатываем сложность
            HandleDifficulty(difficulty,deltaTime);
            
            // Обрабатываем покупателей
            UpdateCustomers(deltaTime);

            if (timeOpacity < 1)
                HandleProke(difficulty, deltaTime);
        }

        private void TestFunction()
        {
        }

        private void UpdateText(TMP_Text text)
        {
            float hour, minutes;
            hourTimer.GetWorldTime(out hour,out minutes);
            

            text.text = $"{hour.ToString(0)}:{minutes.ToString(0)}," +
                        $" opacity: {hourTimer.Opacity.ToString(2)}," +
                        $" curve: {curveHolder.GetCurveValue(hourTimer.Opacity).ToString(2)}";
        }

        /// <summary>
        /// Обновляет текущее время и возвращает opacity
        /// </summary>
        private float UpdateTime(float deltaTime)
        {
            hourTimer.UpdateTimer(deltaTime, timeScale);

            return hourTimer.Opacity;
        }

        /// <summary>
        /// Обрабатывает действия в зависимости от сложности
        /// </summary>
        /// <param name="difficulty">Сложность от 0-1</param>
        private void HandleDifficulty(float difficulty, float deltaTime)
        {
            //Debug.Log(difficulty.ToString(2));
        }

        private void UpdateCustomers(float deltaTime)
        {
            for (int i = 0; i < customers.Count; i++)
            {
                var customer = customers[i];
                customer.UpdateBehaviour(deltaTime);
            }
        }

        /// <summary>
        /// Обрабатывает шансы 
        /// </summary>
        private void HandleProke(float difficulty, float deltaTime)
        {
            prokeDelayCurrent -= deltaTime;
            if (prokeDelayCurrent <= 0)
            {
                prokeDelayCurrent = prokeDelay;

                var modifiedChance = Mathf.Lerp(minChance, maxChance, difficulty);
                var isProked = RandomUtils.ProkeChance(modifiedChance);

                if (isProked)
                {
                    OnProkeEvent();
                    Debug.LogAssertion($"Proked {modifiedChance.ToString(1)}" +
                                       $" : {difficulty.ToString(1)}");
                }
                else
                {
                    Debug.LogAssertion("Not proked");
                }
            }
        }

        /// --------------------CUSTOMERS
        
        private void AddCustomer(CustomerBehaviour customer)
        {
            if (customer != null)
            {
                customer.OnDestroyCall += RemoveCustomer;
                customer.Position = spawnPoint.position;
                customer.Bar = this;
                customers.Add(customer);
            }
        }

        private void RemoveCustomer(AiBehaviour customer)
        {
            var cust = customer as CustomerBehaviour;
            if (cust != null)
                customers.Remove(cust as CustomerBehaviour);
        }

        /// <summary>
        /// Создает случайного покупателя
        /// </summary>
        public void SpawnRandomCustomer()
        {
            var cust = CustomersFactory.CreateCustomerBehaviour();

            if (cust != null)
                AddCustomer(cust);
        }
        
        /// --------------------- CHAIRS

        /// <summary>
        /// Установка стульев. Стульям присваивается номер и выгоняются сидящие.
        /// </summary>
        public void SetupChairs()
        {
            // Находим все стулья со столами
            Chairs = FindObjectsOfType<Chair>().Where(x =>
            {
               x.Setup();
               return x.HasTable == true;
            }).ToList();
            
            freeChairs = new RandomList<Chair>(Chairs);

            for (var i = 0; i < Chairs.Count; i++)
            {
                var chair = Chairs[i];
                chair.SetSerialId(i);
                chair.KickSitting();
                chair.IsRequested = false;
            }
        }

        /// <summary>
        /// Запрос свободного стула. Если стульев нет, то будет возвращено null
        /// </summary>
        /// <returns></returns>
        public Chair RequestFreeChair()
        {
            var chair = freeChairs.Next();

            if (chair == null)
                return null;

            chair.IsRequested = true;
            return chair;
        }

        /// <summary>
        /// Запрос свободного стула. Если совпадений нет, то будет возвращено null.
        /// Если стул занят, то будет вызван метод KickSitting
        /// </summary>
        /// <param name="serialId">Номер стула в баре</param>
        /// <returns></returns>
        public Chair RequestChairBySerialId(int serialId)
        {
            var chair = Chairs.FirstOrDefault(x => x.SerialId == serialId);

            if (chair == null)
                return null;
            
            chair.KickSitting();
            chair.IsRequested = true;
            return chair;
        }

        /// <summary>
        /// Добавление освободившегося стула
        /// </summary>
        public void ReturnReleasedChair(Chair chair)
        {
            if (chair == null)
                return;
            
            chair.IsRequested = false;
            freeChairs.TemporaryCollection.Add(chair);
        }

        /// <summary>
        /// Возвращает путь до стула. Если пути нет, то будет возвращено null
        /// </summary>
        /// <param name="selfPosition">Позиция ищущего</param>
        /// <param name="chair">Искомый стул</param>
        /// <returns></returns>
        public List<Vector3> GetPathToChair(Vector3 selfPosition, Chair chair)
        {
            var nearestCellPosition = SceneGrid.Instance.GetNearestCellCentralized(selfPosition);
            
            var path = SceneGrid.Instance.FindPath(nearestCellPosition, chair.transform.position);
            return path;
        }

        /// <summary>
        /// Возвращает случайный путь по бару. Может вернуть null
        /// </summary>
        public List<Vector3> GetFreeroamPath(Vector3 selfPosition)
        {
            var chair = Chairs.Random();
            
            var nearestCellPosition = SceneGrid.Instance.GetNearestCellCentralized(selfPosition);
            var nearestCellChairPosition = SceneGrid.Instance.
                GetNearestCellCentralized(chair.transform.position);
            
            var path = SceneGrid.Instance.FindPath(nearestCellPosition,nearestCellChairPosition);
            return path;
        }

        /// <summary>
        /// Возвращает путь до выхода из бара
        /// </summary>
        public List<Vector3> GetPathToExit(Vector3 selfPosition)
        {
            if (spawnPoint == null)
                return null;
            
            var nearestCellPosition = SceneGrid.Instance.GetNearestCellCentralized(selfPosition);
            var nearestCellSpawnPosition = SceneGrid.Instance.GetNearestCellCentralized(spawnPoint.position);
            
            
            var path = SceneGrid.Instance.FindPath(nearestCellPosition, nearestCellSpawnPosition);
            path.Add(spawnPoint.position);
            return path;
        }

        public void OnDrawGizmos()
        {
            if (SceneGrid.Instance != null && spawnPoint != null)
            {
                var spawnPosition = SceneGrid.Instance.GetNearestCellCentralized(spawnPoint.position);
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(spawnPoint.position,spawnPosition);
            }
        }
    }
}