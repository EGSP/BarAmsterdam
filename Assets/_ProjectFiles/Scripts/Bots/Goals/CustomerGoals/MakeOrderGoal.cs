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

        public override Goal Execute(AiUpdateData updateData)
        {
            throw new System.NotImplementedException();
        }
    }
}