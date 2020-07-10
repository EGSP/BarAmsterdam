using System;
using System.Collections.Generic;
using Bots.Behaviours;
using UnityEngine;

namespace Bots.Goals.CustomerGoals
{
    public class FindPlaceGoal: CustomerGoal
    {
        public override Type GoalType => typeof(FindPlaceGoal);
        public FindPlaceGoal(ICustomerBehaviour customer) : base(customer)
        {
        }

        public override Goal Execute(AiUpdateData updateData)
        {
            var chair = Customer.RequestedChair;
            if (chair == null)
            {
                chair = Customer.Bar.RequestFreeChair();

                if (chair == null)
                {
                    // Debug.Log("Свободный стул не был найден");
                    return FailedGoal;
                }
            }
            
            var path = Customer.Bar.GetPathToChair(
                Customer.Transform.position, chair);
                
            if (path == null)
            {
                Debug.Log($"Стул свободен, но пути к нему нет {Customer.RequestedChair.name}");
                return FailedGoal;
            }
                
            Customer.RequestedChair = chair;
            Customer.Path = path;
            // Debug.Log("Путь до стула найден");
            return NextGoal;
           
        }
    }
}