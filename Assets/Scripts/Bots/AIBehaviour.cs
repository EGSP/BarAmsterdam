using Bots.Goals;
using Bots.States;
using UnityEngine;

namespace Bots
{
    public class AiBehaviour: MonoBehaviour
    {
        /// <summary>
        /// Текущая цель
        /// </summary>
        public Goal CurrentGoal { get; private set; }
        
        /// <summary>
        /// Текущее состояние поведения
        /// </summary>
        public AiBehaviourState CurrentState { get; private set; }


        private AiUpdateData updateData;
        
        private void Awake()
        {
            updateData = new AiUpdateData();
            updateData.Parent = this;

            ChangeState(new BaseBehaviourState(this));
        }

        public void Update()
        {
            updateData.DeltaTime = Time.deltaTime;
            CurrentState.UpdateState(updateData);
        }

        /// <summary>
        /// Меняет текущее состояние
        /// </summary>
        /// <param name="newState">Новое состояние</param>
        public void ChangeState(AiBehaviourState newState)
        {
            if (newState != null)
            {
                CurrentState = newState;
            }
            else
            {
                if (CurrentState == null)
                    CurrentState = new BaseBehaviourState(this);
            }
        }

        /// <summary>
        /// Устанавливает новую цель
        /// </summary>
        /// <param name="newGoal">Новая цель</param>
        public void SetGoal(Goal newGoal)
        {
            CurrentGoal = newGoal;
        }

        /// <summary>
        /// Добавляет новую цель в последовательность
        /// </summary>
        /// <param name="nextGoal"></param>
        public void AppendGoal(Goal nextGoal)
        {
            CurrentGoal.Join(nextGoal);
        }
    }
}
