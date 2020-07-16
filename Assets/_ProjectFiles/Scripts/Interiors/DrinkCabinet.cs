using System.Collections;
using System;
using System.Collections.Generic;
using Core;
using Player.Controllers;
using Player.PlayerCursors;
using Player.PlayerStates;
using UnityEngine;

using Items;
using Items.Factory;
using Items.MonoItems;
using Items.MonoItems.Consumables;

namespace Interiors
{
    public class DrinkCabinet : TableTop
    {
        public string drinkId;

        private Drink drinkpPrefab;

        private void Awake()
        {
            var prefabInfo = FoodFactory.GetFoodById<Drink>(drinkId);
            if (prefabInfo == null)
                return;

            drinkpPrefab = prefabInfo.Food as Drink;
        }

        public override IItem TakeItemByDistance(Vector3 initiatorPosition)
        {
            if (drinkpPrefab == null)
                return null;
            
            var bottle = Instantiate(drinkpPrefab, new Vector3(0,0,0), Quaternion.identity);
            
            return bottle;
        }

        public bool CompareBottles(Bottle bottle)
        {
            if (bottle.IsFull && drinkpPrefab.DrinkType == bottle.DrinkType)
                return true;

            return false;
        }

        /// <summary>
        /// Уничтожение бутылки
        /// </summary>
        /// <param name="bottleToDestroy">Уничтожаемая бутылка</param>
        public void DestroyBottle(Bottle bottleToDestroy)
        {
            Destroy(bottleToDestroy.gameObject);
        }
    }
}
