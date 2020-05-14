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
    public class DrinkСabinet : TableTop
    {
        [SerializeField] private GameObject drink;
        
        /// <summary>
        /// Ориентация по вертикали от -1 до 1
        /// </summary>
        public int verOrientation { get; private set; } = -1;

        /// <summary>
        /// Ориентация по горизонтали от -1 до 1
        /// </summary>
        public int horOrientation { get; private set; } = 0;

        public override MonoItem TakeItemByDistance(Vector3 initiatorPosition, bool remove = true)
        {
            Debug.Log(initiatorPosition - transform.position == new Vector3(horOrientation, verOrientation * 0.5f, 0));
            var bottle = Instantiate(drink, new Vector3(0,0,0), Quaternion.identity).GetComponent<Bottle>();
            // var bottle = Instantiate(drink, new Vector3(0,0,0), Quaternion.identity);
            Debug.Log("bottle");
            Debug.Log(bottle);
            // bottle.transform.parent = playerController.transform;

            return bottle;
            // return new BaseState(playerController);
        }

        public override void AddItem(MonoItem item)
        {
            Bottle bottle = (Bottle) item;
            Debug.Log(bottle.DrinkType);
            Debug.Log(drink.GetComponent<Bottle>().DrinkType);
            
            if (bottle.DrinkType == drink.GetComponent<Bottle>().DrinkType)
                UnityEngine.Object.Destroy(bottle.gameObject);
            else
                throw new Exception("This Bottle can't be in this Cabinet");
        }
    }
}
