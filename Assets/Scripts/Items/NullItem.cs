using Player.PlayerStates;
using Player.Controllers;

namespace Items
{
    /// <summary>
    /// Нулевой объект, возвращающий базовое состояние
    /// </summary>
    public class NullItem : IItem
    {
        public string ID => "NullItem";

        public PlayerState GetPlayerState(PlayerController playerController)
        {
            return new BaseState(playerController);
        }
    }
}