using System;
using System.IO;
using Bots.ActionObjects;
using Bots.Behaviours;
using Items.MonoItems.Consumables;

namespace Bots.Goals.CustomerGoals
{
    public class WaitOrderGoal: CustomerGoal
    {
        public override Type GoalType => typeof(WaitOrderGoal);
        public WaitOrderGoal(ICustomerBehaviour customer) : base(customer)
        {
            
        }

        private float waitingTime;

        public override void Awake()
        {
            waitingTime = 0;
        }

        public override Goal Execute(AiUpdateData updateData)
        {
             var table = Customer.RequestedChair.table;
            
             // Нет стола, чтобы ждать заказ
            if (table == null)
                return FailedGoal;
                        
            // Заказа нет
            if (Customer.Order == null)
                return FailedGoal;
            
            // Время вышло
            if (waitingTime > Customer.Order.WaitingTime)
                return CancleOrder();
            
            // Сслыка на предмет
            var neededItem = table.FindItemByExpression<Consumable>(x =>
            {
                if (x.IsFull && x.ID == Customer.Order.FoodInfo.Food.ID)
                    return true;

                return false;
            });

            // Нашли предмет
            if (neededItem != null)
            {
                // Реальный предмет, получаемый по ссылке
                var orderItem = table.TakeItemByReference<Consumable>(neededItem);
                Customer.OrderItem = orderItem;
                return NextGoal;
            }

            // Ждем
            waitingTime += updateData.DeltaTime;
            return this;
        }

        private Goal CancleOrder()
        {
            Customer.Order.Cancel();
            return FailedGoal;
        }
    }
}