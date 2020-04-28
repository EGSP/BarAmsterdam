using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Gasanov.SpeedUtils
{
    /// <summary>
    /// Вызывает метод каждый Update пока возвращаемое значение == false
    /// </summary>
    public class FunctionUpdater
    {
        /// <param name="updateFunction">Обновляемый метод</param>
        /// <param name="functionName">Название метода (опционально)</param>
        public FunctionUpdater(Func<float, bool> updateFunction, string functionName = "defaultName")
        {
            this.updateFunction = updateFunction;
            this.functionName = functionName;
            this.isActive = true;
        }

        /// <summary>
        /// Обновляемая функция
        /// </summary>
        private Func<float, bool> updateFunction;

        /// <summary>
        /// Активно ли сейчас обновление функции
        /// </summary>
        public bool isActive { get; private set; }

        /// <summary>
        /// Название функции
        /// </summary>
        public string functionName { get; private set; }

        /// <summary>
        /// Приостанавливает обновление функции
        /// </summary>
        public void Pause()
        {
            isActive = false;
        }

        /// <summary>
        /// Продолжает обновление функции
        /// </summary>
        public void Resume()
        {
            isActive = true;
        }

        /// <summary>
        /// Уничтожает текущий FucntionUpdater
        /// </summary>
        public void DestroySelf()
        {
            RemoveUpdater(this);
        }

        private void Update(float deltaTime)
        {
            if (!isActive) return;

            var ended = updateFunction(deltaTime);

            if (ended == true)
            {
                DestroySelf();
            }
        }

        //
        // STATIC 
        //

        private static List<FunctionUpdater> functionUpdaters;

        /// <summary>
        /// Инициализирует хук и список 
        /// </summary>
        private static void InitialzeIfNeeded()
        {
            if (FunctionUpdaterHook.Instance == null)
            {
                var hook = new GameObject("FunctionUpdaterHook_Inst");

                hook.AddComponent<FunctionUpdaterHook>();

                functionUpdaters = new List<FunctionUpdater>();
            }
        }

        /// <summary>
        /// Уничтожает все выполняемые FunctionUpdater с таким же названием
        /// </summary>
        private static void DestroyAllByName(string functionName)
        {
            InitialzeIfNeeded();
            for (int i = 0; i < functionUpdaters.Count; i++)
            {
                if (functionUpdaters[i].functionName == functionName)
                {
                    functionUpdaters[i].DestroySelf();
                    i--;
                }
            }
        }

        /// <summary>
        /// Уничтожает первый выполняемый FunctionUpdater с таким же названием
        /// </summary>
        private static void DestroyFirstByName(string functionName)
        {
            InitialzeIfNeeded();
            for (int i = 0; i < functionUpdaters.Count; i++)
            {
                if (functionUpdaters[i].functionName == functionName)
                {
                    functionUpdaters[i].DestroySelf();
                    i--;

                    return;
                }
            }
        }

        /// <summary>
        /// Создает новый FunctionUpdater и привязывает его к циклу обновления
        /// </summary>
        /// <param name="updateAction">Обновляемая функция</param>
        /// <param name="actionName">Название обновляемой функции</param>
        /// <param name="destroyAllSame">Нужно ли уничтожить все прошлые экземпляры?</param>
        /// <returns></returns>
        public static FunctionUpdater Create(Func<float, bool> updateFunction, string actionName, bool destroyAllSame = false)
        {
            InitialzeIfNeeded();

            if (destroyAllSame)
            {
                DestroyAllByName(actionName);
            }

            var actionUpdater = new FunctionUpdater(updateFunction, actionName);
            ActionUpdaterHook.Instance.OnUpdate += actionUpdater.Update;

            functionUpdaters.Add(actionUpdater);

            return actionUpdater;
        }

        public static FunctionUpdater Create(Func<float, bool> updateFunction)
        {
            return Create(updateFunction, "defaultName", false);
        }

        /// <param name="destroyAllDefaults">Нужно ли уничтожить все экземпляры со стандартным названием?</param>
        public static FunctionUpdater Create(Func<float, bool> updateFunction, bool destroyAllDefaults)
        {
            return Create(updateFunction, "defaultName", destroyAllDefaults);
        }

        /// <summary>
        /// Убирает из списка обновления FunctionUpdater
        /// </summary>
        public static void RemoveUpdater(FunctionUpdater functionUpdater)
        {
            InitialzeIfNeeded();
            functionUpdaters.Remove(functionUpdater);
        }
    }

    public class FunctionUpdaterHook : MonoBehaviour
    {
        public static FunctionUpdaterHook Instance;

        public event Action<float> OnUpdate = delegate { };

        private void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);

            Instance = this;
        }

        public void Update()
        {
            OnUpdate(Time.deltaTime);
        }
    }
}
