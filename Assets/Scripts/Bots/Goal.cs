﻿using System;
using System.Collections.Generic;
using System.Linq;
using Gasanov.Exceptions;

namespace Bots.Goals
{
    // Цель может циклить только в саму себя
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
        /// Добавляет цель в конец цепочки. Все зацикленности будут устранены.
        /// Возвращает новую цель
        /// </summary>
        /// <param name="goal">Новая цель</param>
        /// <returns></returns>
        public Goal Append(Goal goal)
        {
            // Если текущая цель есть эта
            if (NextGoal == this)
            {
                NextGoal = goal;
                return goal;
            }
            
            var allGoals = new List<Goal>();
            
            allGoals.Add(this);
            var currentGoal = NextGoal;

            while (currentGoal != null)
            {
                allGoals.Add(currentGoal);
                if (currentGoal.NextGoal == null)
                {
                    currentGoal.NextGoal = goal;
                    return goal;
                }
                
                var coincidence = allGoals.FirstOrDefault(x => x==currentGoal.NextGoal);
                
                // Если следующая задача уже была, то меняем следующую на новую
                if (coincidence != null)
                {
                    currentGoal.NextGoal = goal;
                    return goal;
                }

                currentGoal = currentGoal.NextGoal;
                allGoals.Add(currentGoal);                
                
            }

            this.NextGoal = goal;
            return goal;
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
        /// Вставляет новую цель между целями и возвращает прежнюю следующую цель.
        /// Если следующих целей не было, то вернет новую цель
        /// </summary>
        public Goal Insert(Goal newGoal)
        {
            if (NextGoal == null)
                return Join(newGoal);

            var oldNextGoal = Split();

            NextGoal = newGoal;
            newGoal.NextGoal = oldNextGoal;
            return oldNextGoal;
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