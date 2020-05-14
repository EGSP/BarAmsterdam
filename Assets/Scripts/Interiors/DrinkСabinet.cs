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
        [SerializeField] private Bottle drink;
        
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
            var bottle = Instantiate(drink, new Vector3(0,0,0), Quaternion.identity);
            // var bottle = Instantiate(drink, new Vector3(0,0,0), Quaternion.identity);
            Debug.Log("bottle");
            Debug.Log(bottle);
            // bottle.transform.parent = playerController.transform;

            return bottle;
            // return new BaseState(playerController);
        }

        /// <summary>
        /// Уничтожает добавляемый объект
        /// </summary>
        public override void AddItem(IItem item)
        {
            var monoItem  = item as MonoItem;
            
            //----- Кабинет наследован от обычного стола, а обычный стол не возвращает информацию
            //----- об успешном добавлении предмета. Мы в обычный стол предмет положили и забыли. 
            //----- к тому же если у нас не бутылка, то ошибка выводит неверную информацию.
            //----- А если ты сводишься на то, что мы знаем в момент вызова о типе DrinkCabinet,
            //----- то тогда почему бы не сделать отдельный метод UtilizeBottle 
            //----- На данный момент кабинет просто удаляет все складываемые предметы
            
            // Debug.Log(bottle.DrinkType);
            // Debug.Log(drink.GetComponent<Bottle>().DrinkType);
            
            // if (bottle.DrinkType == drink.DrinkType)
            //     Destroy(bottle.gameObject);
            // else
            //     throw new Exception("This Bottle can't be in this Cabinet");

            if (monoItem != null)
            {
                Destroy(monoItem.gameObject);        
                Debug.Log($"{monoItem.name} has destroyed");
            }
        }
    }
}
