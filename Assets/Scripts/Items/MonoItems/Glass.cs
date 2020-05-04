using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Items.MonoItems;
using Player.Controllers;
using Player.PlayerStates;

namespace Items.MonoItems
{
    public class Glass : MonoItem
    {
        public Bottle.Drink DrinkType
        {
            get => drinkType;
        }

        [SerializeField] private Bottle.Drink drinkType;

        public bool isFill;

        public override PlayerState GetPlayerState(PlayerController playerController)
        {
            return new BaseState(playerController);
            // return new GlassState(playerController, this);
        }

        public void Pour(Bottle bottle)
        {
            if (bottle.isFull)
            {
                isFill = true;
                drinkType = bottle.DrinkType;
            }
        }

        public void toClean()
        {
            isFill = false;
            drinkType = Bottle.Drink.Clean;
        }
    }
}
