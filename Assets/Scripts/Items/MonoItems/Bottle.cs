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

        [SerializeField] private Drink drinkType;

        private int maxFullness;
        private int currentFullness;

        public bool isFull
        {
            get => currentFullness == maxFullness ? true : false;
        }

        public bool isFill
        {
            get => currentFullness != 0 ? true : false;
        }

        public override PlayerState GetPlayerState(PlayerController playerController)
        {
            return new BottleState(playerController, this);
        }

        public void PourOff(Glass glass)
        {
            if (isFull)
            {
                currentFullness = currentFullness > 0 ? currentFullness - 1 : 0;
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