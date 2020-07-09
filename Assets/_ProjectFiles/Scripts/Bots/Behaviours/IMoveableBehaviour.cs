using System.Collections.Generic;
using UnityEngine;

namespace Bots.Behaviours
{
    public interface IMoveableBehaviour
    {
        Transform Transform { get; }
        
        /// <summary>
        /// Скорость движения
        /// </summary>
        float MoveTime { get; }
        
        /// <summary>
        /// Текущий путь куда-либо. Может отсутствовать
        /// </summary>
        List<Vector3> Path{ get; set; }

        void Dispose();
    }
}