using Bots.Behaviours;

namespace Bots.Goals.CustomerGoals
{
    public class MoveFreeroamGoal: CustomerGoal
    {
        public MoveFreeroamGoal(ICustomerBehaviour customer) : base(customer)
        {
        }

        public override Goal Execute(AiUpdateData updateData)
        {
            throw new System.NotImplementedException();
        }
    }
}