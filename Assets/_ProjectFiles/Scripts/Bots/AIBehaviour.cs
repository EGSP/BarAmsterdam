using System;
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

        [SerializeField] private string goalType;
        
        /// <summary>
        /// Текущее состояние поведения
        /// </summary>
        public AiBehaviourState CurrentState { get; private set; }

        public event Action<AiBehaviour> OnDestroyCall = delegate { };


        private AiUpdateData updateData;
        
        public void AwakeBehaviour()
        {
            updateData = new AiUpdateData();
            updateData.Parent = this;

            ChangeState(new BaseBehaviourState(this));
        }

        public void UpdateBehaviour(float deltaTime)
        {
            Debug.Log(CurrentGoal?.GoalType);
            goalType = CurrentGoal?.GoalType.ToString();
            updateData.DeltaTime = deltaTime;
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
        /// Добавляет новую цель в цепочку. Возвращает новую цель
        /// </summary>
        /// <param name="nextGoal"></param>
        public Goal AppendGoal(Goal nextGoal)
        {
            return CurrentGoal.Append(nextGoal);
        }

        public virtual void DestroyBehaviour()
        {
            OnDestroyCall(this);
        }
    }
}
