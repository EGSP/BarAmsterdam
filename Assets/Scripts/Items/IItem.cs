
using Player;

namespace Items
{
    /// <summary>
    /// Интерфейс определяющий предмет
    /// </summary>
    public interface IItem : IPlayerStateChanger
    {
        /// <summary>
        /// Идентификатор предмета
        /// </summary>
        string ID { get; }
    }
}