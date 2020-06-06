using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Gasanov.SpeedUtils
{
    /// <summary>
    /// Вызывает метод каждый Update пока работает таймер
    /// </summary>
    public class ActionUpdater
    {
        /// <param name="updateAction">Обновляемый метод</param>
        /// <param name="timerTime">Время таймера, по истечении которого метод прекратит работу</param>
        /// <param name="actionName">Название метода (опционально)</param>
        public ActionUpdater(Action<float> updateAction, float timerTime, string actionName = "defaultName")
        {
            this.updateAction = updateAction;
            this.timerTime = timerTime;
            this.actionName = actionName;
            this.isActive = true;
        }

        /// <summary>
        /// Обновляемый метод
        /// </summary>
        private Action<float> updateAction;

        /// <summary>
        /// Текущее время таймера
        /// </summary>
        private float timerTime;

        /// <summary>
        /// Активно ли сейчас обновление метода
        /// </summary>
        public bool isActive { get; private set; }

        /// <summary>
        /// Название метода
        /// </summary>
        public string actionName { get; private set; }

        /// <summary>
        /// Приостанавливает обновление метода
        /// </summary>
        public void Pause()
        {
            isActive = false;
        }

        /// <summary>
        /// Продолжает обновление метода
        /// </summary>
        public void Resume()
        {
            isActive = true;
        }

        /// <summary>
        /// Уничтожает текущий ActionUpdater
        /// </summary>
        public void DestroySelf()
        {
            RemoveUpdater(this);
        }

        private void Update(float deltaTime)
        {
            if (!isActive) return;

            timerTime -= deltaTime;
            if (timerTime < 0)
            {
                DestroySelf();
            }
            else
            {
                updateAction(deltaTime);
            }
        }

        //
        // STATIC 
        //

        private static List<ActionUpdater> actionUpdaters;

        /// <summary>
        /// Инициализирует хук и список 
        /// </summary>
        private static void InitialzeIfNeeded()
        {
            if (ActionUpdaterHook.Instance == null)
            {
                var hook = new GameObject("ActionUpdaterHook_Inst");

                hook.AddComponent<ActionUpdaterHook>();

                actionUpdaters = new List<ActionUpdater>();
            }

            // Помещено в первое условие
            //if (actionUpdaters == null)
            //    actionUpdaters = new List<ActionUpdater>();
        }

        /// <summary>
        /// Уничтожает все выполняемые ActionUpdater с таким же названием
        /// </summary>
        private static void DestroyAllByName(string actionName)
        {
            InitialzeIfNeeded();
            for (int i = 0; i < actionUpdaters.Count; i++)
            {
                if (actionUpdaters[i].actionName == actionName)
                {
                    actionUpdaters[i].DestroySelf();
                    i--;
                }
            }
        }

        /// <summary>
        /// Уничтожает первый выполняемый ActionUpdater с таким же названием
        /// </summary>
        private static void DestroyFirstByName(string actionName)
        {
            InitialzeIfNeeded();
            for (int i = 0; i < actionUpdaters.Count; i++)
            {
                if (actionUpdaters[i].actionName == actionName)
                {
                    actionUpdaters[i].DestroySelf();
                    i--;

                    return;
                }
            }
        }

        /// <summary>
        /// Создает новый ActionUpdater и привязывает его к циклу обновления
        /// </summary>
        /// <param name="updateAction">Обновляемый метод</param>
        /// <param name="timerTime">Время таймера, по истечении которого updateAction прекратит работу</param>
        /// <param name="actionName">Название обновляемого метода</param>
        /// <param name="destroyAllSame">Нужно ли уничтожить все прошлые экземпляры?</param>
        /// <returns></returns>
        public static ActionUpdater Create(Action<float> updateAction, float timerTime, string actionName, bool destroyAllSame = false)
        {
            InitialzeIfNeeded();

            if (destroyAllSame)
            {
                DestroyAllByName(actionName);
            }

            var actionUpdater = new ActionUpdater(updateAction, timerTime, actionName);
            ActionUpdaterHook.Instance.OnUpdate += actionUpdater.Update;

            actionUpdaters.Add(actionUpdater);

            return actionUpdater;
        }

        public static ActionUpdater Create(Action<float> updateAction, float timerTime)
        {
            return Create(updateAction, timerTime, "defaultName", false);
        }

        /// <param name="destroyAllDefaults">Нужно ли уничтожить все экземпляры со стандартным названием?</param>
        public static ActionUpdater Create(Action<float> updateAction, float timerTime, bool destroyAllDefaults)
        {
            return Create(updateAction, timerTime, "defaultName", destroyAllDefaults);
        }

        /// <summary>
        /// Убирает из списка обновления ActionUpdater
        /// </summary>
        public static void RemoveUpdater(ActionUpdater actionUpdater)
        {
            InitialzeIfNeeded();
            actionUpdaters.Remove(actionUpdater);
        }
    }

    /// <summary>
    /// Класс позволяющий использовать обновления Update для ActionUpdater
    /// </summary>
    public class ActionUpdaterHook: MonoBehaviour
    {
        public static ActionUpdaterHook Instance;

        public event Action<float> OnUpdate = delegate { };

        private void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);

            Instance = this;
        }

        public void Update()
        {
            OnUpdate(UnityEngine.Time.deltaTime);
        }
    }
}
