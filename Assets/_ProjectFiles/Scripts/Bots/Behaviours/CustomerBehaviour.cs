using System;
using UnityEngine;
using World;

namespace Bots.Behaviours
{
    public class CustomerBehaviour: AiBehaviour, ICustomerBehaviour
    {
        /// <summary>
        /// Бар, к которому привязан бот
        /// </summary>
        public Bar Bar { get; set; }

        public void OnEnable()
        {
            Debug.Log("Бот покупатель появился!");
        }
    }
}