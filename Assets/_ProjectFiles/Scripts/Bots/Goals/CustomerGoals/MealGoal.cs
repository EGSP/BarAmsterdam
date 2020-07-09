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
            throw new System.NotImplementedException();
        }
    }
}