using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public static class DeviceInput
    {
        /// <summary>
        /// Текущая карта кнопок
        /// </summary>
        private static DeviceMapping map;

        /// <summary>
        /// Получения значений ввода кнопки ("Z"), которая отвечает за удерживание предмета
        /// </summary>
        /// <returns></returns>
        public static bool GetHandleButtonDown()
        {
            return Input.GetKeyDown(map.Zbutton);
        }

        /// <summary>
        /// Получение значений ввода кнопки ("X"), которая отвечает за взаимодействие с предметом
        /// </summary>
        /// <returns></returns>
        public static bool GetActionButtonDown()
        {
            return Input.GetKeyDown(map.Xbutton);
        }

        /// <summary>
        /// Получение значений ввода кнопки ("Space"), которая отвечает за дополнительное взаимодействие
        /// </summary>
        /// <returns></returns>
        public static bool GetExtraButtonDown()
        {
            return Input.GetKeyDown(map.SpaceButton);
        }

        /// <summary>
        /// Получение значений ввода удерживания по горизонтальной оси от -1 до 1
        /// </summary>
        /// <returns></returns>
        public static float GetHorizontalAxis()
        {
            float left = Input.GetKey(map.HorizontalLeft) ? -1f : 0f;
            float right = Input.GetKey(map.HorizontalRight) ? 1f : 0f;

            return left + right;
        }

        /// <summary>
        /// Получение значений ввода нажатия по горизонтальной оси от -1 до 1
        /// </summary>
        /// <returns></returns>
        public static float GetHorizontalAxisDown()
        {
            float left = Input.GetKeyDown(map.HorizontalLeft) ? -1f : 0f;
            float right = Input.GetKeyDown(map.HorizontalRight) ? 1f : 0f;

            return left + right;
        }

        /// <summary>
        /// Получение значений ввода отжатия по горизонтальной оси от -1 до 1
        /// </summary>
        /// <returns></returns>
        public static float GetHorizontalAxisUp()
        {
            float left = Input.GetKeyUp(map.HorizontalLeft) ? -1f : 0f;
            float right = Input.GetKeyUp(map.HorizontalRight) ? 1f : 0f;

            return left + right;
        }

        /// <summary>
        /// Получение значений ввода удерживания по вертикальной оси от -1 до 1
        /// </summary>
        /// <returns></returns>
        public static float GetVerticalAxis()
        {
            float down = Input.GetKey(map.VerticalDown) ? -1f : 0f;
            float up = Input.GetKey(map.VerticalUp) ? 1f : 0f;

            return down + up;
        }

        /// <summary>
        /// Получение значений ввода нажатия по вертикальной оси от -1 до 1
        /// </summary>
        /// <returns></returns>
        public static float GetVerticalAxisDown()
        {
            float down = Input.GetKeyDown(map.VerticalDown) ? -1f : 0f;
            float up = Input.GetKeyDown(map.VerticalUp) ? 1f : 0f;

            return down + up;
        }

        /// <summary>
        /// Получение значений ввода отжатия по вертикальной оси от -1 до 1
        /// </summary>
        /// <returns></returns>
        public static float GetVerticalAxisUp()
        {
            float down = Input.GetKeyUp(map.VerticalDown) ? -1f : 0f;
            float up = Input.GetKeyUp(map.VerticalUp) ? 1f : 0f;

            return down + up;
        }

        /// <summary>
        /// Изменение карты кнопок
        /// </summary>
        /// <param name="newMap"></param>
        public static void SetMapping(DeviceMapping newMap)
        {
            if (newMap != null)
                map = newMap;
        }
    }
}