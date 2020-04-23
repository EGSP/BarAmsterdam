using System.Collections;
using System.Collections.Generic;
using Player;
using Player.PlayerStates;
using UnityEngine;

namespace Interiors
{
    public class Box : Interior
    {
        public override PlayerState GetPlayerState(PlayerController playerController)
        {
            return new BaseState(playerController);
        }
    }
}
