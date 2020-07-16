using System;
using Bots.Behaviours;

namespace Bots.Goals.CustomerGoals
{
   
    public class MakeOrderGoal: CustomerGoal
    {
        public override Type GoalType => typeof(MakeOrderGoal);
        public MakeOrderGoal(ICustomerBehaviour customer) : base(customer)
        {
        }

        public override void Awake()
        {
            Customer.Order = null;
        }

        public override Goal Execute(AiUpdateData updateData)
        {
            if (Customer.Order == null)
            {
                // Получаем случайный заказ
                var order = Customer.Bar.MakeRandomOrder(Customer);

                if (order == null)
                    return FailedGoal;

                Customer.Order = order;
                return NextGoal;
            }

            // Уже есть заказ, значит цель выполнена
            return NextGoal;
        }
    }
}