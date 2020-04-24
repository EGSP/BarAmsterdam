using System.Collections;

using System.Linq;

using Items.MonoItems;

namespace Player.PlayerCursors
{
    /// <summary>
    /// Класс отвечает за тип, который будет обработан курсором.
    /// </summary>
    public class CursorEnumerator : IEnumerator
    {
        /// <summary>
        /// Перечисляемый объект
        /// </summary>
        private readonly ICursorEnumerable CursorEnumerable;

        /// <summary>
        /// Enumerator текущей перебираемой коллекции
        /// </summary>
        private IEnumerator cursorCollectionEnumerator;

        public CursorEnumerator(ICursorEnumerable cursorEnumerable)
        {
            if (cursorEnumerable == null)
                throw new System.Exception("cursorEnumerable is null in CursorEnumerator.cs");

            CursorEnumerable = cursorEnumerable;

            var collection = CursorEnumerable.GetCollection();
            cursorCollectionEnumerator = collection.GetEnumerator();

            Count = collection.Count();
        }


        public MonoItem Current => cursorCollectionEnumerator.Current as MonoItem;

        object IEnumerator.Current => cursorCollectionEnumerator.Current;

        /// <summary>
        /// Количество объектов в перечисляемой коллекции
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Возвращает true, если мы вышли на новый круг
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            var end = cursorCollectionEnumerator.MoveNext();

            if (end == false)
            {
                cursorCollectionEnumerator.Reset();
                cursorCollectionEnumerator.MoveNext();
            }

            return end;
        }

        public void Reset()
        {
            cursorCollectionEnumerator.Reset();
        }

        /// <summary>
        /// Обновляет Enumerator в соответствии с типом
        /// </summary>
        private void ChangeEnumratorType<T>() where T : MonoItem
        {
            var collection = CursorEnumerable.GetCollection().Where(x => x as T != null);
            cursorCollectionEnumerator = collection.GetEnumerator();

            Count = collection.Count();
        }

        public void Dispose()
        {
        }
    }
}
