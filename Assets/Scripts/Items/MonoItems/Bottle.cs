using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Items.MonoItems;
using Player.Controllers;
using Player.PlayerStates;

namespace Items.MonoItems
{
    public class Bottle : MonoItem
    {
        public enum Drink
        {
            Whisky,
            Vodka,
            Gin,
            Brandy,
            Wine,
            Beer,
            Clean
        }

        public Drink DrinkType { get => drinkType;}
        
        private float BottlePrice;
        private int BottleVolume;
        private float BottleWaitingTime;
        private float BottleEatingTime;
        private float BottleOrderChance;
        
        [SerializeField] private Drink drinkType;

        private int currentFullness;

        public Bottle()
        {
            BottlePrice = FoodInfo.BottlePrice[drinkType];
            BottleVolume = FoodInfo.BottleVolume[drinkType];
            BottleWaitingTime = FoodInfo.BottleWaitingTime[drinkType];
            BottleEatingTime = FoodInfo.BottleEatingTime[drinkType];
            BottleOrderChance = FoodInfo.BottleOrderChance[drinkType];
            
            currentFullness = BottleVolume;
        }

        public bool isFill
        {
            get => currentFullness == BottleVolume ? true : false;
        }

        public bool isFull
        {
            get => currentFullness != 0 ? true : false;
        }

        public override PlayerState GetPlayerState(PlayerController playerController)
        {
            return new BottleState(playerController, this);
        }

        public void PourOff(Glass glass)
        {
            if (isFull && glass.DrinkType == Bottle.Drink.Clean)
            {
                Debug.Log(currentFullness);
                currentFullness = currentFullness > 0 ? currentFullness - 1 : 0;
                Debug.Log(currentFullness);
                glass.Pour(this);
            }
        }

        public PlayerState ThrowOut(PlayerController playerController)
        {
            Destroy(this);
            return new BaseState(playerController);
        } 
    }
    

}