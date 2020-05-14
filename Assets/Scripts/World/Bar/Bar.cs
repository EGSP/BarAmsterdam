using System;
using Gasanov.SpeedUtils;
using Gasanov.SpeedUtils.Time;
using TMPro;
using UnityEngine;

namespace World
{
    public class Bar : MonoBehaviour
    {
        [Header("Настройки времени")]
        [SerializeField][Range(0,24)] private int startHour;
        [SerializeField][Range(0,60)] private int startMinute;

        [SerializeField][Range(0,24)] private int endHour;
        [SerializeField][Range(0,60)] private int endMinute;
        [Range(0,10)] public float timeScale;

        [Space(10)] [Header("Настройки сложности")]
        [SerializeField] private CurveHolder curveHolder;

        

        /// <summary>
        /// Таймер дня
        /// </summary>
        private HourTimer hourTimer;
        
        
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