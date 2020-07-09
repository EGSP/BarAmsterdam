using System;
using Bots.Behaviours;

namespace Bots.Goals.CustomerGoals
{
    public abstract class CustomerGoal : Goal
    {
        public override Type GoalType => typeof(CustomerGoal);
        
        public readonly ICustomerBehaviour Customer;
        public CustomerGoal(ICustomerBehaviour customer)
        {
            Customer = customer;
        }
    }
}