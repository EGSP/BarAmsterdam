using System;
using Bots.Behaviours;

namespace Bots.Goals.CustomerGoals
{
    public class MealGoal : CustomerGoal
    {
        public override Type GoalType => typeof(MealGoal);
        public MealGoal(ICustomerBehaviour customer) : base(customer)
        {
        }

        public override Goal Execute(AiUpdateData updateData)
        {
            if (Customer.Order == null)
                return FailedGoal;

            if (Customer.OrderItem == null)
                return CancleOrder();

            var orderItem = Customer.OrderItem;
            UnityEngine.GameObject.Destroy(orderItem.gameObject);
            orderItem = null;
            
            Customer.Order.Cancel();

            return NextGoal;
        }
        
        private Goal CancleOrder()
        {
            Customer.Order.Cancel();
            return FailedGoal;
        }
    }
}