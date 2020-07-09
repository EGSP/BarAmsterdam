using System;
using System.Collections.Generic;
using System.Linq;
using Bots.Behaviours;
using Bots.Goals;
using Bots.Goals.CustomerGoals;
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
            if(IsPrefabsLoaded == false)
                LoadCustomersPrefabs();
            
            if (_customersPrefabs.Count == 0)
                return null;
            
            var index = UnityEngine.Random.Range(0, _customersPrefabs.Count);
            var customer = UnityEngine.Object.Instantiate(_customersPrefabs[index]);
            
            

            // Создаем все цели и потом цепим их друг к другу
            var findGoal = new FindPlaceGoal(customer);
            var freeroamGoal = new MoveFreeroamGoal(customer);
            var exitGoal = new ExitFromBarGoal(customer);
            var moveToGoal = new MoveToPlaceGoal(customer);
            moveToGoal.Join(exitGoal);
            
            findGoal.JoinFailed(freeroamGoal);
            findGoal.Join(moveToGoal);
            
            freeroamGoal.JoinFailed(exitGoal);
            freeroamGoal.Join(findGoal);
            
            customer.SetGoal(findGoal);
            customer.OnDestroyCall += DestroyBehaviour;
            
            customer.AwakeBehaviour();
            return customer;
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

        /// <summary>
        /// Уничтожает бота обычным способом UnityEngine.Object.Destroy
        /// </summary>
        public static void DestroyBehaviour(AiBehaviour behaviour)
        {
            if (behaviour != null)
            {
                UnityEngine.Object.Destroy(behaviour.gameObject);
            }
        }
    }
}