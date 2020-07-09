namespace Bots.States
{
    /// <summary>
    /// Базовое состояние поведения, которое просто вызывает цель при ее наличии
    /// </summary>
    public class BaseBehaviourState: AiBehaviourState
    {
        public BaseBehaviourState(AiBehaviour parent) : base(parent)
        {
            
        }
        
        public override void UpdateState(AiUpdateData updateData)
        {
            // Получаем цель
            var goal = Parent.CurrentGoal;
            
            // Проверяем наличие цели
            if (goal != null)
            {
                var newGoal = goal.Execute(updateData);

                if (newGoal == null)
                    return;
                    
                // Если это новая цель
                if (newGoal != goal)
                {
                    newGoal.Awake();
                }
                
                // Меняем цель на новую
                Parent.SetGoal(newGoal);
            }
        }
    }
}