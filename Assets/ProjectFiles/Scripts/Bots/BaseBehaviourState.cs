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
                // Меняем цель на новую
                Parent.SetGoal(goal.Execute(updateData));
            }
        }
    }
}