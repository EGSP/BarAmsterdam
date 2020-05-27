using System;
using Gasanov.Exceptions;
using Gasanov.Extensions;
using Gasanov.SpeedUtils;
using Gasanov.SpeedUtils.Time;
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

        public FoodInfo FoodInformation { get; private set; }

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

    }
}