using UnityEngine;

namespace Items.MonoItems.Consumables
{
    public abstract class Drink: Consumable
    {
        public enum DrinkTypes
        {
            Whisky,
            Vodka,
            Gin,
            Brandy,
            Wine,
            Beer,
            None
        }

        public override string ID
        {
            get => id + DrinkType.ToString();
        }

        /// <summary>
        /// Тип напитка
        /// </summary>
        public virtual DrinkTypes DrinkType { get=>drinkType; protected set=> drinkType = value; }
        [SerializeField] private DrinkTypes drinkType = DrinkTypes.None;

        /// <summary>
        /// Очищает предмет и устанавливает тип напитка None
        /// </summary>
        public override void Clean()
        {
            base.Clean();
            drinkType = DrinkTypes.None;
        }
    }
}