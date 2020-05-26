using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Items.MonoItems;
using Items.MonoItems.Consumables;
using Player.Controllers;
using Player.PlayerStates;

namespace Items.MonoItems
{
    public class Glass : Drink
    {

        public override PlayerState GetPlayerState(PlayerController playerController)
        {
            return new GlassState(playerController, this);
        }
        
        /// <summary>
        /// Наполняет одну единицу объема и меняет тип в зависимости от бутылки
        /// </summary>
        /// <param name="bottle">Наполняющиая бутылка</param>
        public void FillByBottle(Bottle bottle)
        {
            Fill(1);
            DrinkType = bottle.DrinkType;
        }
    }
}
