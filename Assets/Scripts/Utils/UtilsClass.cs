using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

/*
 * За основу был взят, улучшен и переработан пример утилит CodeMonkeys  
 */

namespace Gasanov.SpeedUtils
{
    /// <summary>
    /// Класс вспомогательных инструментов для ускорения разработки
    /// </summary>
    public static class UtilsClass
    {

        /// <summary>
        /// Создает текст в мировом пространстве
        /// </summary>
        /// <param name="text">Отображаемый текст</param>
        /// <param name="parent">Родительский объект</param>
        /// <param name="localPosition">Позиция в пространстве родителя</param>
        /// <param name="fontSize">размер шрифта</param>
        /// <param name="color">Цвет текста. По умолчанию белый</param>
        /// <param name="widthAndHeight">Ширина и высота текста</param>
        /// <param name="textAlignment">Ориентация текста</param>
        /// <returns></returns>
        public static TMP_Text CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 5,
            Color color = default(Color), Vector2 widthAndHeight = default(Vector2), TextAlignmentOptions textAlignment = TextAlignmentOptions.Center)
        {
            GameObject gameObject = new GameObject("World_TextPro", typeof(TextMeshPro));

            Transform transform = gameObject.transform;
            transform.SetParent(parent);
            transform.localPosition = localPosition;

            TMP_Text textPro = gameObject.GetComponent<TMP_Text>();
            textPro.text = text;
            textPro.fontSize = fontSize;
            if (widthAndHeight == Vector2.zero)
                widthAndHeight = new Vector2(1, 0.5f);

            textPro.rectTransform.sizeDelta = widthAndHeight;
            textPro.alignment = textAlignment;


            textPro.color = color;


            return textPro;
        }

        /// <summary>
        /// Создает всплывающий текст в мировом пространстве
        /// </summary>
        /// <param name="finalPosition">Конечная позиция текста</param>
        /// <param name="popupTime">Время за которое текст переместится в конечную позицию</param>
        public static void CreateWorldTextPopup(string text, Transform parent, Vector3 localPosition, int fontSize,
            Color color, Vector3 finalPosition, float popupTime)
        {
            if (popupTime <= 0f)
                popupTime = 1f;

            // Inversed time
            var speed = 1 / popupTime;

            var textMesh = CreateWorldText(text, parent, localPosition, fontSize, color);
            var textTransform = textMesh.transform;

            FunctionUpdater.Create((deltaTime) =>
            {
                textTransform.position = Vector3.MoveTowards(textTransform.position, finalPosition, speed * deltaTime);

                popupTime -= deltaTime;
                if (popupTime <= 0f)
                {
                    Object.Destroy(textTransform.gameObject);
                    return true;
                }
                else
                {
                    return false;
                }
            }, "popupText");
        }

        /// <summary>
        /// Создает всплывающий текст в мировом пространстве
        /// </summary>
        public static void CreateWorldTextPopup(string text,Vector3 localPosition,Vector3 finalPosition)
        {
            CreateWorldTextPopup(text, null, localPosition, 5, Color.white, finalPosition, 1f);
        }

        /// <summary>
        /// Создает всплывающий текст в мировом пространстве
        /// </summary>
        public static void CreateWorldTextPopup(string text, Vector3 localPosition)
        {
            CreateWorldTextPopup(text, localPosition, localPosition + Vector3.up);
        }


        /// <summary>
        /// Получение координат мыши в мировом пространстве с Z
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetMouseWorldPositionWithZ()
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
        {
            var worldPos = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPos;
        }

        /// <summary>
        /// Получение позиции мыши в мировых координатах без учета Z
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetMouseWorldPosition()
        {
            var mouseWorldPosition = GetMouseWorldPositionWithZ();
            mouseWorldPosition.z = 0f;

            return mouseWorldPosition;
        }

    }// Class
    
}
