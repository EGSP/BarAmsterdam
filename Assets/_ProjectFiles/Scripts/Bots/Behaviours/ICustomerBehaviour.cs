using World;

namespace Bots.Behaviours
{
    public interface ICustomerBehaviour
    {
        /// <summary>
        /// Бар, к которому привязан бот
        /// </summary>
        Bar Bar { get; }
    }
}