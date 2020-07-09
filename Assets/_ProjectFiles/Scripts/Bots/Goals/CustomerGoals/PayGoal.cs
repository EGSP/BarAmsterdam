using System;
using Bots.Behaviours;

namespace Bots.Goals.CustomerGoals
{
    public class PayGoal: CustomerGoal
    {
        public override Type GoalType => typeof(PayGoal);
        public PayGoal(ICustomerBehaviour customer) : base(customer)
        {
        }

        public override Goal Execute(AiUpdateData updateData)
        {
            throw new System.NotImplementedException();
        }
    }
}