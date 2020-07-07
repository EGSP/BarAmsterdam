using System;
using System.Collections.Generic;
using System.Linq;
using Bots.Behaviours;
using UnityEngine;
using Random = System.Random;

namespace Bots.Factory
{
    public static class CustomersFactory
    {
        static CustomersFactory()
        {
            _customersPrefabs = new List<CustomerBehaviour>();
        }

        /// <summary>
        /// Были ли загружены префабы
        /// </summary>
        public static bool IsPrefabsLoaded { get; private set; }
        
        /// <summary>
        /// Список загруженных префабов
        /// </summary>
        private static List<CustomerBehaviour> _customersPrefabs;

        /// <summary>
        /// Загружает все префабы покупателей
        /// </summary>
        /// <param name="path"></param>
        public static void LoadCustomersPrefabs(bool reload = false, string path = "Factory/Customers/")
        {
            if (IsPrefabsLoaded && !reload)
                return;

            _customersPrefabs = Resources.LoadAll(path, typeof(CustomerBehaviour))
                .Cast<CustomerBehaviour>().ToList();

            if (_customersPrefabs.Count == 0)
            {
                Debug.Log("Префабы покупателей небыли загружены");
                return;
            }

            IsPrefabsLoaded = true;
        }
        
        /// <summary>
        /// Создает случайного покупателя. Может вернуть null, если префабы не загружены 
        /// </summary>
        public static CustomerBehaviour CreateCustomerBehaviour()
        {
            if (_customersPrefabs.Count == 0)
                return null;
            
            var index = UnityEngine.Random.Range(0, _customersPrefabs.Count);
            var instance = UnityEngine.Object.Instantiate(_customersPrefabs[index]);
            
            return instance;
        }

        /// <summary>
        /// Выгружает бота из памяти
        /// </summary>
        public static void UnloadBehaviour(UnityEngine.Object behaviour)
        {

            if (behaviour != null)
            {
                Resources.UnloadAsset(behaviour);
            }
        }
    }
}