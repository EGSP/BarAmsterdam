using System.Collections.Generic;
using UnityEngine;

namespace Bots.Goals.Utils
{
    public static class GoalsUtils
    {
        /// <summary>
        /// Перемещение вдоль пути. Сразу изменяет переданную позицию.
        /// Возвращает true, когда путь пройден
        /// </summary>
        /// <param name="path">Путь точек</param>
        /// <param name="nodeIndex">Индекс точки, с которой продолжится движение</param>
        /// <param name="selfPosition">Позиция идущего</param>
        /// <param name="moveTime">Скорость движения</param>
        /// <param name="deltaTime"></param>
        public static bool MoveAlongPath(List<Vector3> path, ref int nodeIndex, Transform selfTransform,
            float moveTime, float deltaTime)
        {
            if (path == null)
                return true;
            
            if (nodeIndex >= path.Count)
                return true;

            var targetPosition = path[nodeIndex];
            var distance = (selfTransform.position - targetPosition).sqrMagnitude;

            if (distance <= float.Epsilon)
            {
                // Переходим на следующую ноду
                nodeIndex++;
                return false;
            }
            
            var nextPosition = Vector3.MoveTowards(selfTransform.position, targetPosition,
                (1/moveTime) * deltaTime);

            selfTransform.position = nextPosition;

            return false;
        }
    }
}