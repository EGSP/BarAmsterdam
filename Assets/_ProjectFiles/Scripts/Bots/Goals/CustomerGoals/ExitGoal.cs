using Bots.Behaviours;

namespace Bots.Goals.CustomerGoals
{
    public class ExitGoal : CustomerGoal
    {
        public ExitGoal(ICustomerBehaviour customer) : base(customer)
        {
        }

        public override Goal Execute(AiUpdateData updateData)
        {
            throw new System.NotImplementedException();
        }
    }
}