namespace Core
{
    // Объект имеющий ориентацию в пространстве
    public interface IHaveOrientation
    {
        /// <summary>
        /// Ориентация в пространстве
        /// </summary>
        Orientation Orientation { get; }
    }
}