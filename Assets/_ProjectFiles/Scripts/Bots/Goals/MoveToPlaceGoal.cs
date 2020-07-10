using System;
using Bots.Behaviours;
using Bots.Goals.Utils;
using UnityEngine;

namespace Bots.Goals
{
    public class MoveToPlaceGoal: Goal
    {
        public override Type GoalType => typeof(MoveToPlaceGoal);
        public readonly IMoveableBehaviour Moveable;
        public MoveToPlaceGoal(IMoveableBehaviour moveable)
        {
            Moveable = moveable;
        } 

        private int currentNode;

        public override Goal Execute(AiUpdateData updateData)
        {
            // Debug.Log("Бот идет");
            if (Moveable.Path == null)
                return FailedGoal;

            if (Moveable.Path.Count == 0)
                return FailedGoal;

            var nextPosition = Moveable.Transform.position;
            if (GoalsUtils.MoveAlongPath(Moveable.Path, ref currentNode, Moveable.Transform,
                Moveable.MoveTime, updateData.DeltaTime))
            {
                // Debug.Log("Бот успешно дошел");
                return NextGoal;
            }
            
            return this;
        }
    }
}