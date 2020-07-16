using System;
using System.Collections.Generic;
using System.Linq;
using Bots.Behaviours;
using Gasanov.SpeedUtils.FileManagement;
using Gasanov.SpeedUtils.RandomUtilities;
using Items.MonoItems.Consumables;
using UnityEngine;

namespace Items.Factory
{
    public static class FoodFactory
    {
        
        public static readonly string DataPath = "Consumables/";
        
        static FoodFactory()
        {
            _foodInfos = new List<FoodInfo>();
        }

        public static bool IsPrefabsLoaded;
        
        /// <summary>
        /// Префабы еды
        /// </summary>
        private static List<FoodInfo> _foodInfos;

        public static void LoadFoodPrefabs(bool reload = false, string path = "Factory/Food/")
        {
            if (IsPrefabsLoaded && !reload)
                return;

            if (reload)
            {
                for (var i = 0; i < _foodInfos.Count; i++)
                {
                    Resources.UnloadAsset(_foodInfos[i].Food);
                }
                
                _foodInfos = new List<FoodInfo>();
            }
            
            var consumablesPrefabs = Resources.LoadAll(path, typeof(Consumable))
                .Cast<Consumable>().ToList();
            
            for (var i = 0; i < consumablesPrefabs.Count; i++)
            {
                var prefab = consumablesPrefabs[i];
            
                var price = SaveSystem.LoadProperty<float>(prefab.ID, DataPath, "price",
                    true);
                var chance = SaveSystem.LoadProperty<float>(prefab.ID, DataPath, "chance",
                    true);
                
                var foodInfo = new FoodInfo(prefab,price,chance);
                _foodInfos.Add(foodInfo);
            }
        }

        /// <summary>
        /// Возвращет случайную еду
        /// </summary>
        /// <returns></returns>
        public static FoodInfo GetRandomFood()
        {
            LoadFoodPrefabs();
            
            return RandomUtils.SelectByWeight(_foodInfos, x => x.ChanceToOrder);
        }

        /// <summary>
        /// Возвращает еду по идентификатору. Может вернуть null.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static FoodInfo GetFoodById(string id)
        {
            LoadFoodPrefabs();

            var coincidence = _foodInfos.FirstOrDefault(x => x.Food.ID == id);

            return coincidence;
        }

        /// <summary>
        /// Возвращает еду по идентификатору и Типу. Может вернуть null.
        /// </summary>
        /// <param name="id"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static FoodInfo GetFoodById<T>(string id) where T: Consumable
        {
            LoadFoodPrefabs();
            
            var coincidence = _foodInfos.FirstOrDefault(x => 
                x.Food.ID == id && (x.Food as T) != null);

            return coincidence;
        }
        
        
        // [SerializeField] private List<Drink> drinksPrefab;
        //
        // /// <summary>
        // /// Данные о напитках (префаб, цена, шанс) 
        // /// </summary>
        // private List<Tuple<Drink, float, float>> drinksData;
        //
        // private void Awake()
        // {
        //     Initialzie();
        // }
        //
        // private void Initialzie()
        // {
        //     drinksData = new List<Tuple<Drink, float, float>>();
        //     for (var i = 0; i < drinksPrefab.Count; i++)
        //     {
        //         var prefab = drinksPrefab[i];
        //
        //         var price = SaveSystem.LoadProperty<float>(prefab.ID, DataPath, "price",
        //             true);
        //         var chance = SaveSystem.LoadProperty<float>(prefab.ID, DataPath, "chance",
        //             true);
        //
        //         drinksData.Add(new Tuple<Drink, float, float>(prefab, price, chance));
        //
        //     }
        //
        //     //var tuple = GetRandomDrink();
        // }
        //
        // /// <summary>
        // /// Возвращает случайный кортеж (префаб, цена, шанс)
        // /// </summary>
        // public Tuple<Drink, float, float> GetRandomDrink()
        // {
        //     return RandomUtils.SelectByWeight(drinksData, x => x.Item3);
        // }
    }
}