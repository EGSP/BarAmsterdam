using System.Collections.Generic;
using Interiors;
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

        void Dispose();
    }
}