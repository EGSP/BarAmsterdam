using System.Collections;
using System;
using System.Collections.Generic;

using Player.Controllers;
using Player.PlayerCursors;
using Player.PlayerStates;
using UnityEngine;

using Items;
using Items.MonoItems;

namespace Interiors
{
    public class DrinkCabinet : TableTop
    {
        [SerializeField] private Bottle bottlePrefab;
        
        /// <summary>
        /// Ориентация по вертикали от -1 до 1
        /// </summary>
        public int verOrientation { get; private set; } = -1;

        /// <summary>
        /// Ориентация по горизонтали от -1 до 1
        /// </summary>
        public int horOrientation { get; private set; } = 0;

        public override IItem TakeItemByDistance(Vector3 initiatorPosition)
        {
            Debug.Log(initiatorPosition - transform.position == new Vector3(horOrientation, verOrientation * 0.5f, 0));
            var bottle = Instantiate(bottlePrefab, new Vector3(0,0,0), Quaternion.identity);
            // var bottle = Instantiate(drink, new Vector3(0,0,0), Quaternion.identity);
            Debug.Log("bottle");
            Debug.Log(bottle);
            // bottle.transform.parent = playerController.transform;

            return bottle;
            // return new BaseState(playerController);
        }

        public bool CompareBottles(Bottle bottle)
        {
            if (bottle.IsFull && bottlePrefab.DrinkType == bottle.DrinkType)
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
