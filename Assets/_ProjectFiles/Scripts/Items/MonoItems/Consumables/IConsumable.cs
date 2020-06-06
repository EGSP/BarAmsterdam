using UnityEngine;

namespace Items.MonoItems.Consumables
{
    /// <summary>
    /// Любой предмет, который может быть потреблен
    /// </summary>
    public interface IConsumable
    {
        /// <summary>
        /// Изображение предмета
        /// </summary>
        Sprite Icon { get; }
        
        /// <summary>
        /// Время потребления
        /// </summary>
        float ConsumptionTime { get; }
        
        /// <summary>
        /// Максимальное количество раз, которое можно употреблять данный объект
        /// </summary>
        int Volume { get; }
        
        /// <summary>
        /// Полный ли предмет
        /// </summary>
        bool IsFull { get; }
        
        /// <summary>
        /// Пустой ли предмет
        /// </summary>
        bool IsEmpty { get; }


        /// <summary>
        /// Наполняет предмет
        /// </summary>
        /// <param name="count">Количество наполнений</param>
        void Fill(int count);
        
        /// <summary>
        /// Потребляет один раз предмет
        /// </summary>
        void Consume();

        /// <summary>
        /// Полностью потребляет весь предмет
        /// </summary>
        void Clean();
    }
}