using Bots.Behaviours;

namespace Bots.Goals.CustomerGoals
{
    public class PayGoal: CustomerGoal
    {
        public PayGoal(ICustomerBehaviour customer) : base(customer)
        {
        }

        public override Goal Execute(AiUpdateData updateData)
        {
            throw new System.NotImplementedException();
        }
    }
}