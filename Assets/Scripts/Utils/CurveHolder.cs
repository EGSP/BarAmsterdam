using UnityEngine;

namespace Gasanov.SpeedUtils
{
    [System.Serializable]
    public class CurveHolder
    {
        /// <summary>
        /// Кривая значений
        /// </summary>
        [SerializeField] private AnimationCurve curve;
        
        /// <summary>
        /// Местоположение точки на кривой
        /// </summary>
        public float CurvePoint { get; private set; }

        public readonly float MaxPointValue = 1;

        /// <summary>
        /// Смещение точки кривой
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public float MovePoint(float delta)
        {
            CurvePoint += delta;

            if (CurvePoint > MaxPointValue)
            {
                CurvePoint = MaxPointValue;
            }
            else if(CurvePoint<0)
            {
                CurvePoint = 0;
            }

            return GetCurveValue(CurvePoint);
        }

        /// <summary>
        /// Получения значения кривой по точке
        /// </summary>
        /// <param name="point">Точка по которой получаем значение</param>
        public float GetCurveValue(float point)
        {
            return curve.Evaluate(point);
        }

        /// <summary>
        /// Возвращает точку в исходное положение на кривой
        /// </summary>
        public void ResetPoint() => CurvePoint = 0;
    }
}