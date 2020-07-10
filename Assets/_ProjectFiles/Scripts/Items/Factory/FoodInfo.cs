
using Items.MonoItems.Consumables;

namespace Items.Factory
{
    public class FoodInfo
    {
        public FoodInfo(Consumable food, float price, float chanceToOrder)
        {
            Food = food;
            Price = price;
            ChanceToOrder = chanceToOrder;
        }
        
        /// <summary>
        /// Экземпляр еды, служит префабом для FoodFactory
        /// </summary>
        public Consumable Food { get; private set; }

        /// <summary>
        /// Цена за этот тип еды
        /// </summary>
        public float Price { get; private set; }
        
        /// <summary>
        /// Шанс еды быть заказнной клиентом
        /// </summary>
        public float ChanceToOrder { get; private set; }
    }
}