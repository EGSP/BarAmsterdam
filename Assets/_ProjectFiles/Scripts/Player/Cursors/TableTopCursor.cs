using System.Collections.Generic;
using Items.MonoItems;
using UnityEngine;

namespace Player.PlayerCursors
{
    public class TableTopCursor : MonoBehaviour
    {
        /// <summary>
        /// Игровой объект курсора
        /// </summary>
        [SerializeField] private Transform cursor;
        
        /// <summary>
        /// Текущий перечислитель курсора
        /// </summary>
        public CursorEnumerator CursorEnumerator { get; private set; }

        /// <summary>
        /// Активен ли сейчас курсор
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Установка текущего перечислителя
        /// </summary>
        /// <param name="cursorEnumerator"></param>
        public void SetCursorEnumerator(CursorEnumerator cursorEnumerator)
        {
            CursorEnumerator = cursorEnumerator;
            if (Next() == true)
            {
                cursor.gameObject.SetActive(true);
                IsActive = true;
            }
        }

        /// <summary>
        /// Выбирает следующий предмет. Возвращает false, если нет следующего предмета
        /// </summary>
        /// <returns></returns>
        public bool Next()
        {
            if (CursorEnumerator.Count == 0)
            {
                cursor.gameObject.SetActive(false);

                IsActive = false;
                return false;
            }

            CursorEnumerator.MoveNext();

            var item = CursorEnumerator.Current;

            cursor.transform.position = item.transform.position;

            IsActive = true;
            return true;
        }

        public void Cancel()
        {
            cursor.gameObject.SetActive(false);
            IsActive = false;
        }

        /// <summary>
        /// Возвращает выделенный предмет. Может вернуть null
        /// </summary>
        /// <returns></returns>
        public MonoItem GetSelectedItem()
        {
            if (CursorEnumerator.Count == 0)
                return null;

            return CursorEnumerator.Current;
        }
    }
}

