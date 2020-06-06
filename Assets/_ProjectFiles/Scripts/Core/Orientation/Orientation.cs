using UnityEngine;

namespace Core
{
    /// <summary>
    /// Класс отвечающий за ориентацию в пространстве предметов
    /// </summary>
    public class Orientation
    {
        public Orientation()
        {
            Direction = Vector3.right;
        }
        // Направление ориентации. Не равно нулевым координатам
        public Vector3 Direction { get; private set; }
        
        /// <summary>
        /// Локальное право
        /// </summary>
        public Vector3 LocalRight => Vector3.Cross(Direction.normalized,Vector3.forward);
        
        /// <summary>
        /// Локальное лево
        /// </summary>
        public Vector3 LocalLeft => -1 * Vector3.Cross(Direction.normalized, Vector3.forward);

        /// <summary>
        /// Локальный зад
        /// </summary>
        public Vector3 LocalBack => -1 * Direction;

        public void SetDirection(float horizontal, float vertical)
        {
            if (horizontal == 0 && vertical == 0)
            {
                Direction = Vector3.right;
                return;
            }
            
            Direction = new Vector3(horizontal,vertical);
        }

        public void SetDirection(Vector3 direction)
        {
            if (direction == Vector3.zero)
            {
                Direction = Vector3.right;
                return;
            }

            Direction = direction;
        }
    }
}