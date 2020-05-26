using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Items.MonoItems;
using Items.MonoItems.Consumables;
using Player.Controllers;
using Player.PlayerStates;

namespace Items.MonoItems
{
    public class Bottle : Drink
    {
        public override PlayerState GetPlayerState(PlayerController playerController)
        {
            return new BottleState(playerController, this);
        }

        /// <summary>
        /// Наполняет стакан на одну единицу объема 
        /// </summary>
        /// <param name="glass">Наполняемый стакан</param>
        public void FillGlass(Glass glass)
        {
            if (IsEmpty)
                return;
            
            // Если стакан не полон и тип напитка совпадает с нашим
            if(glass.IsFull != true && 
               (glass.DrinkType == DrinkTypes.None || glass.DrinkType == DrinkType))
            {
                glass.FillByBottle(this);
                Consume();
            }
        }
    }
    

}