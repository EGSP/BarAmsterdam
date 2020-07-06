using Bots.Behaviours;

namespace Bots.Goals.CustomerGoals
{
    public class WaitOrderGoal: CustomerGoal
    {
        public WaitOrderGoal(ICustomerBehaviour customer) : base(customer)
        {
            
        }

        public override Goal Execute(AiUpdateData updateData)
        {
            throw new System.NotImplementedException();
        }
    }
}