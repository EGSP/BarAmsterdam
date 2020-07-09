using System;
using System.Collections.Generic;
using Bots.Behaviours;
using Bots.Goals.Utils;
using UnityEngine;

namespace Bots.Goals.CustomerGoals
{
    public class MoveFreeroamGoal: CustomerGoal
    {
        public override Type GoalType => typeof(MoveFreeroamGoal);
        public readonly CustomerBehaviour CustomerBehaviour;
        public MoveFreeroamGoal(CustomerBehaviour customerBehaviour) : base(customerBehaviour)
        {
            CustomerBehaviour = customerBehaviour;
        }

        private int currentNode;
        public override void Awake()
        {
            CustomerBehaviour.Path = null;
        }

        public override Goal Execute(AiUpdateData updateData)
        {
            // Ищем путь
            if (CustomerBehaviour.Path == null)
            {
                CustomerBehaviour.Path = Customer.Bar.GetFreeroamPath(Customer.Transform.position);
            
                // Если путей вообще нет
                if (CustomerBehaviour.Path == null)
                {
                    return FailedGoal;
                }
            }
            
            // Если закончили прогулку
            if (GoalsUtils.MoveAlongPath(CustomerBehaviour.Path, ref currentNode, CustomerBehaviour.Transform,
                CustomerBehaviour.MoveTime, updateData.DeltaTime))
            {
                // Если стул найден после прогулки
                if (Customer.Bar.FreeChairAvailable)
                {
                    // Получаем найденный стул
                    var chair = Customer.Bar.RequestFreeChair();
                    Customer.RequestedChair = chair;
                    
                    return NextGoal;
                }

                return FailedGoal;
            }

            return this;
        }

        
    }
}