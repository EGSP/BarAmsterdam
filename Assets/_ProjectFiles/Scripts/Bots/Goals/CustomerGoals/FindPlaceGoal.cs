using Bots.Behaviours;

namespace Bots.Goals.CustomerGoals
{
    public class FindPlaceGoal: CustomerGoal
    {
        public FindPlaceGoal(ICustomerBehaviour customer) : base(customer)
        {
        }

        public override Goal Execute(AiUpdateData updateData)
        {
            throw new System.NotImplementedException();
        }
    }
}