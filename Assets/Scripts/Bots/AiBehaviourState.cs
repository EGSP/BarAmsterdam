using Bots;

namespace Bots.States
{
    public abstract class AiBehaviourState
    {
        protected readonly AiBehaviour Parent;

        public AiBehaviourState(AiBehaviour parent)
        {
            Parent = parent;
        }
        
        public abstract void UpdateState(AiUpdateData updateData);
    }
}