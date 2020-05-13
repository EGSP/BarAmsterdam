using System;
using Gasanov.SpeedUtils;
using Gasanov.SpeedUtils.Time;
using TMPro;
using UnityEngine;

namespace World
{
    public class Bar : MonoBehaviour
    {
        [SerializeField][Range(0,24)] private int startHour;
        [SerializeField][Range(0,60)] private int startMinute;

        [SerializeField][Range(0,24)] private int endHour;
        [SerializeField][Range(0,60)] private int endMinute;

        [Range(0,5)] public float timeScale;

        // Таймер дня
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
            hourTimer.GetTime(out hour,out minutes);
            
            // Debug.Log($"{elapsedMinutes}: {hour}: {minutes}");

            text.text = $"{hour.ToString(0)}:{minutes.ToString(0)}";
        }

    }
}