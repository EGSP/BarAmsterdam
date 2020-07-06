using Bots.Behaviours;

namespace Bots.Goals.CustomerGoals
{
    public class MoveToPlaceGoal: CustomerGoal
    {
        public MoveToPlaceGoal(ICustomerBehaviour customer) : base(customer)
        {
        }

        public override Goal Execute(AiUpdateData updateData)
        {
            throw new System.NotImplementedException();
        }
    }
}