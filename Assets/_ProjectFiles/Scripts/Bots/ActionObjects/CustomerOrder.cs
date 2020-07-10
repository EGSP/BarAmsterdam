using Bots.Behaviours;
using Items.Factory;

namespace Bots.ActionObjects
{
    public class CustomerOrder
    {
        public CustomerOrder(ICustomerBehaviour customer, FoodInfo foodInfo, float waitingTime)
        {
            Customer = customer;
            FoodInfo = foodInfo;
            WaitingTime = waitingTime;
        }
        
        /// <summary>
        /// Создатель заказа
        /// </summary>
        public ICustomerBehaviour Customer { get; set; }

        /// <summary>
        /// Идентификатор требуемой еды
        /// </summary>
        public FoodInfo FoodInfo { get; set; }
        
        /// <summary>
        /// Время ожидания заказа клиентом
        /// </summary>
        public float WaitingTime { get; set; }
    }
}