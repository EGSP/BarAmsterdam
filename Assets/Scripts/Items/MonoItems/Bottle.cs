using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Items.MonoItems;
using Player.Controllers;
using Player.PlayerStates;

public class Bottle : MonoItem
{
    public override PlayerState GetPlayerState(PlayerController playerController)
    {
        return new BaseState(playerController);
    }
}
