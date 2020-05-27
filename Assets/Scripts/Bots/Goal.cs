using System;
using Gasanov.Exceptions;

namespace Bots.Goals
{
    public abstract class Goal
    {
        /// <summary>
        /// Следующая цель
        /// </summary>
        protected Goal NextGoal;

        /// <summary>
        /// Добавляет и возвращает следующую цель
        /// </summary>
        /// <param name="goal">Следующая цель</param>
        /// <exception cref="ElementDefineException"></exception>
        public Goal Join(Goal goal)
        {
            if(NextGoal != null)
                throw new ElementDefineException();

            NextGoal = goal;
            return NextGoal;
        }

        /// <summary>
        /// Разъединяет цели, возвращая следующую цель
        /// </summary>
        /// <returns></returns>
        public Goal Split()
        {
            var next = NextGoal;
            NextGoal = null;
            
            return next;
        }

        /// <summary>
        /// Выполнение цели. Возвращает новую цель 
        /// </summary>
        public abstract Goal Execute(AiUpdateData updateData);
    }

    // /// <summary>
    // /// Пустая цель для замены null значений. Возвращает новую цель при ее наличии
    // /// </summary>
    // public class EmptyGoal : Goal
    // {
    //     public override Goal Execute(GoalExecuteData data)
    //     {
    //         if (NextGoal != null)
    //             return NextGoal;
    //
    //         return this;
    //     }
    // }
}