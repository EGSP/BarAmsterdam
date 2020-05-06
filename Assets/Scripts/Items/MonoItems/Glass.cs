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
        public Bottle.Drink DrinkType { get => drinkType; }
        
        private float GlassPrice;
        private float GlassWaitingTime;
        private float GlassEatingTime;
        private float GlassOrderChance;

        [SerializeField] private Bottle.Drink drinkType;

        public Glass()
        {
            GlassPrice = FoodInfo.GlassPrice[drinkType];
            GlassWaitingTime = FoodInfo.GlassWaitingTime[drinkType];
            GlassEatingTime = FoodInfo.GlassEatingTime[drinkType];
            GlassOrderChance = FoodInfo.GlassOrderChance[drinkType];
        }
        public bool isFill;

        public override PlayerState GetPlayerState(PlayerController playerController)
        {
            return new GlassState(playerController, this);
        }

        public void Pour(Bottle bottle)
        {
                isFill = true;
                drinkType = bottle.DrinkType;
        }

        public void toClean()
        {
            isFill = false;
            drinkType = Bottle.Drink.Clean;
        }
    }
}
