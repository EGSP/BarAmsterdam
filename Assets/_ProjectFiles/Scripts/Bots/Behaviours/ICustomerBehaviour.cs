using System.Collections.Generic;
using Bots.ActionObjects;
using Interiors;
using Items.MonoItems.Consumables;
using UnityEngine;
using World;

namespace Bots.Behaviours
{
    public interface ICustomerBehaviour
    {
        Transform Transform { get; }
        
        /// <summary>
        /// Текущий путь куда-либо. Может отсутствовать
        /// </summary>
        List<Vector3> Path{ get; set; } 
        
        /// <summary>
        /// Бар, к которому привязан бот
        /// </summary>
        Bar Bar { get; }
        
        /// <summary>
        /// Запрошенный стул
        /// </summary>
        Chair RequestedChair { get; set; }
        
        /// <summary>
        /// Заказ в баре
        /// </summary>
        CustomerOrder Order { get; set; }

        /// <summary>
        /// Предмет полученный через заказ
        /// </summary>
        Consumable OrderItem { get; set; }

        void Dispose();
    }
}