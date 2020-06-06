using System;
using System.Collections.Generic;
using Items;
using Items.MonoItems;
using UnityEngine;

namespace Interiors
{
    public class Washer: TableTop
    {
        /// <summary>
        /// Стол, на который складываются предметы
        /// </summary>
        [SerializeField] private TableTop output;

        /// <summary>
        /// Время помывки
        /// </summary>
        [SerializeField] private float washTime;

        /// <summary>
        /// Предметы, которые моются
        /// </summary>
        private List<Tuple<MonoItem, float>> itemsAreWashed = new List<Tuple<MonoItem, float>>();
        
        private void Update()
        {
            for (var i = 0; i < itemsAreWashed.Count; i++)
            {
                var tuple = itemsAreWashed[i];
                itemsAreWashed[i] = new Tuple<MonoItem, float>(tuple.Item1,
                    tuple.Item2-Time.deltaTime);

                // Время помывки закончилось
                if (itemsAreWashed[i].Item2 <= 0)
                {
                    Output(itemsAreWashed[i].Item1);
                    itemsAreWashed.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// Выдает предметы
        /// </summary>
        private void Output(MonoItem item)
        {
            TableTop table;
            // Если есть стол выдачи
            if (output != null)
            {
                table = output;
            }
            else
            {
                // Если стола выдачи нет, то выкладываем на свой стол
                table = this;
            }
            
            // Если свободно место
            if (table.PlaceAvailable)
            {
                table.AddItemToFreePlace(item);
                item.gameObject.SetActive(true);
            }
        }

        protected override void AddItem(ItemPlace place, IItem itemToAdd)
        {
            var glass = itemToAdd as Glass;

            if (glass != null)
            {
                glass.transform.parent = transform;
                glass.transform.position = transform.position;
                glass.gameObject.SetActive(false);

                // Время имитирует очередь
                var tuple = new Tuple<MonoItem, float>(glass, washTime*(itemsAreWashed.Count+1));
                itemsAreWashed.Add(tuple);
                
                Debug.Log("Стакан добавлен в очередь помывки");
            }
        }

        /// <summary>
        /// Проверка на стакан
        /// </summary>
        public override bool TypeCompatibility(IItem item)
        {
            if (item is Glass)
                return true;

            return false;
        }
    }
}