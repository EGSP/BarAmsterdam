﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Player.PlayerStates;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        /// <summary>
        /// Радиус обработки столкновений
        /// </summary>
        public float CollisionRadius { get => collisionRadius; }
        [Header("Collision")] [SerializeField] private float collisionRadius;

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
        /// Инвертированное время используемое для передвижения
        /// </summary>
        private float InversedMoveTime { get => 1 / MoveTime; }

        /// <summary>
        /// Перемещается ли в данный момент персонаж
        /// </summary>
        public bool IsMoving { get; private set; }

        private IEnumerator movementRoutine;

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
            updateData = new UpdateData();
            SetState(new BaseState(this));
        }

        // Update is called once per frame
        void Update()
        {
            updateData.deltaTime = Time.deltaTime;
            CurrentPlayerState = CurrentPlayerState.UpdateState(updateData);
        }

        /// <summary>
        /// Движение персонажа в зависимости от нажатых клавиш. Проверки на границы значений клавиш нет.
        /// </summary>
        /// <param name="horizontalInput">Клавиши горизонтального ввода от -1 до 1</param>
        /// <param name="verticalInput">Клавиши вертикального ввода от -1 до 1</param>
        public void Move(int horizontalInput, int verticalInput)
        {
            var horizontalDir = horizontalInput * transform.right * MoveStep;
            var verticalDir = verticalInput * transform.up * MoveStep;

            Vector3 newPosition = transform.position;

            bool horObstacleExist = CheckObstacleByLinecast(transform.position + horizontalDir);
            bool verObstacleExist = CheckObstacleByLinecast(transform.position + verticalDir);


            // Если не можем никуда пойти (true)
            if (horObstacleExist && verObstacleExist)
                return;

            // Если можем идти по диагонали. Нет препятствий по бокам
            if (horObstacleExist == false && verObstacleExist == false)
            {
                bool diaObstacleExist = CheckObstacleByLinecast(transform.position + horizontalDir + verticalDir);

                // Если в точке по диагонали нет препятствий
                if (diaObstacleExist == false)
                {
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

            // Одно из направлений нулевое, но это не страшно
            newPosition += horizontalDir * (horObstacleExist == false ? 1 : 0);
            newPosition += verticalDir * (verObstacleExist == false ? 1 : 0);

            // Сокращаем время поиска нужной корутины
            // При перезначении старая корутина все еще может существовать, пока не доделает свою работу 
            movementRoutine = Movement(newPosition);
            StartCoroutine(movementRoutine);

        }

        /// <summary>
        /// Корутина, передвигающая персонажа в новую позицию
        /// </summary>
        /// <param name="newPosition">Новая позиция</param>
        /// <returns></returns>
        protected IEnumerator Movement(Vector3 newPosition)
        {
            var sqrMagnitude = (transform.position - newPosition).sqrMagnitude;

            IsMoving = true;

            // Пока расстояние больше очень малого значения близкого к нулю
            while (sqrMagnitude > float.Epsilon)
            {
                transform.position = Vector3.MoveTowards(transform.position, newPosition, InversedMoveTime * Time.deltaTime);

                sqrMagnitude = (transform.position - newPosition).sqrMagnitude;

                // Ждем обновления кадра
                yield return null;
            }

            IsMoving = false;
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
        public T GetComponentByLinecast<T>(Vector3 endPosition) where T : Component
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.right * MoveStep);
        }
    }
}