using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using Gasanov.SpeedUtils;

/// <summary>
/// Отладочный класс, имеющий различные вспомогательные функции
/// </summary>
public class SUDebug
{
    /// <summary>
    /// Создает всплывающий текст в позиции мыши
    /// </summary>
    public static void TextPopupMouse(string text)
    {
        UtilsClass.CreateWorldTextPopup(text, UtilsClass.GetMouseWorldPosition());
    }

    /// <summary>
    /// Создает всплывающий текст в мировом пространстве
    /// </summary>
    public static void TextPopup(string text, Vector3 position)
    {
        UtilsClass.CreateWorldTextPopup(text, position);
    }
}

