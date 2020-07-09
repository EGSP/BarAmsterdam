using System;
using Bots.Behaviours;
using Bots.Goals.Utils;
using UnityEngine;

namespace Bots.Goals.CustomerGoals
{
    public class ExitFromBarGoal : Goal
    {
        public override Type GoalType => typeof(ExitFromBarGoal);
        public readonly CustomerBehaviour CustomerBehaviour;
        public ExitFromBarGoal(CustomerBehaviour customerBehaviour)
        {
            CustomerBehaviour = customerBehaviour;
            
        }

        public override void Awake()
        {
            base.Awake();
            // Очищаем прошлый путь
            CustomerBehaviour.Path = null;
            
            CustomerBehaviour.Bar.ReturnReleasedChair(CustomerBehaviour.RequestedChair);
        }

        private int currentNode;
        public override Goal Execute(AiUpdateData updateData)
        {
            var path = CustomerBehaviour.Path;
            // Получаем путь до выхода
            if (path == null)
            {
                path = CustomerBehaviour.Bar.GetPathToExit(CustomerBehaviour.Position);
                if (path == null)
                {
                    Debug.Log("Бот не нашел выход и самоуничтожился");
                    CustomerBehaviour.Dispose();
                    return null;
                }

                CustomerBehaviour.Path = path;
            }

            if (GoalsUtils.MoveAlongPath(CustomerBehaviour.Path, ref currentNode, CustomerBehaviour.Transform,
                CustomerBehaviour.MoveTime, updateData.DeltaTime))
            {
                
                Debug.Log("Бот дошел до выхода и самоуничтожился");
                CustomerBehaviour.Dispose();
                return null;
            }

            Debug.Log("Бот идет на выход");
            return this;
        }
        
        
    }
}