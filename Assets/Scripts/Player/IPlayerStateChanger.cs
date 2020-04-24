using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Player.PlayerStates;
using Player.Controllers;

namespace Player
{
    public interface IPlayerStateChanger
    {
        /// <summary>
        /// Получение нового состояния для игрока, требуемое этим интерьером
        /// </summary>
        PlayerState GetPlayerState(PlayerController playerController);
    }
}
