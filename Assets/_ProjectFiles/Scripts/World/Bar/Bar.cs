using System;
using System.Collections.Generic;
using System.Linq;
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

        [Space(10)] [Header("Настройки объектов")] 
        [SerializeField] private Transform spawnPoint;
        
        public FoodInfo FoodInformation { get; private set; }
        
        
        /// <summary>
        /// Все стулья бара
        /// </summary>
        public List<Chair> Chairs { get; private set; }

        private RandomList<Chair> freeChairs;
        

        /// <summary>
        /// Таймер дня
        /// </summary>
        private HourTimer hourTimer;

        private void Awake()
        {
            if(Instance != null)
                throw new SingletonException<Bar>(this);
            
            Instance = this;

            FoodInformation = GetComponent<FoodInfo>();
            if(FoodInformation == null)
                throw new NullReferenceException();
            
            SetupChairs();
        }
        
        private void Start()
        {
            hourTimer = new HourTimer(startHour,startMinute,endHour,endMinute);
            
            var text = UtilsClass.CreateWorldText("time",Color.white);
            
            ActionUpdater.Create((delta) =>
            {
                hourTimer.UpdateTimer(delta,this.timeScale);
                UpdateText(text);
                
            }, hourTimer.TargetMinutes);
            
            TestFunction();
        }

        private void TestFunction()
        {
            CustomersFactory.LoadCustomersPrefabs();
            var cust = CustomersFactory.CreateCustomerBehaviour();
        }

        private void UpdateText(TMP_Text text)
        {
            float hour, minutes;
            hourTimer.GetWorldTime(out hour,out minutes);
            
            // Debug.Log($"{elapsedMinutes}: {hour}: {minutes}");

            text.text = $"{hour.ToString(0)}:{minutes.ToString(0)}," +
                        $" opacity: {hourTimer.Opacity.ToString(2)}," +
                        $" curve: {curveHolder.GetCurveValue(hourTimer.Opacity).ToString(2)}";
        }


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
            
            freeChairs = new RandomList<Chair>(new List<Chair>());

            for (var i = 0; i < Chairs.Count; i++)
            {
                var chair = Chairs[i];
                chair.SetSerialId(i);
                chair.KickSitting();
                chair.Requested = false;
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

            chair.Requested = true;
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
            chair.Requested = true;
            return chair;
        }

        /// <summary>
        /// Добавление освободившегося стула
        /// </summary>
        public void ReturnReleasedChair(Chair chair)
        {
            chair.Requested = false;
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
            var path = SceneGrid.Instance.FindPath(selfPosition, chair.transform.position);
            return path;
        }

        /// <summary>
        /// Возвращает случайный путь по бару. Может вернуть null
        /// </summary>
        public List<Vector3> GetFreeroamPath(Vector3 selfPosition)
        {
            var chair = Chairs.Random();
            var path = SceneGrid.Instance.FindPath(selfPosition, chair.transform.position);
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