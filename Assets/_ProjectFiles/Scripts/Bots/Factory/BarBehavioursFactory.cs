using System;
using Bots.Behaviours;
using UnityEngine;

namespace Bots.Factory
{
    public static class BarBehavioursFactory
    {
        /// <summary>
        /// Создает случайного покупателя
        /// </summary>
        public static ICustomerBehaviour CreateCustomerBehaviour()
        {
            
            return null;
        }

        /// <summary>
        /// Выгружает бота из памяти
        /// </summary>
        public static void UnloadBehaviour(UnityEngine.Object behaviour)
        {

            if (behaviour != null)
            {
                Resources.UnloadAsset(behaviour);
            }
        }
    }
}