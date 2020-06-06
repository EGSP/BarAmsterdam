using System;
using System.Collections;
using System.Collections.Generic;
using Gasanov.SpeedUtils.FileManagement;
using Gasanov.SpeedUtils.RandomUtilities;
using UnityEngine;

using Items.MonoItems;
using Items.MonoItems.Consumables;

public class FoodInfo: MonoBehaviour
{
    public static readonly string DataPath = "Consumables/";
    
    [SerializeField] private List<Drink> drinksPrefab;

    /// <summary>
    /// Данные о напитках (префаб, цена, шанс) 
    /// </summary>
    private List<Tuple<Drink, float, float>> drinksData;

    private void Awake()
    {
        Initialzie();
    }
    
    private void Initialzie()
    {
        drinksData = new List<Tuple<Drink, float, float>>();
        for (var i = 0; i < drinksPrefab.Count; i++)
        {
            var prefab = drinksPrefab[i];
            
            var price = SaveSystem.LoadProperty<float>(prefab.ID, DataPath,"price",
                true);
            var chance = SaveSystem.LoadProperty<float>(prefab.ID, DataPath, "chance",
                true);
            
            drinksData.Add(new Tuple<Drink, float, float>(prefab,price,chance));
           
        }    
        //var tuple = GetRandomDrink();
    }

    /// <summary>
    /// Возвращает случайный кортеж (префаб, цена, шанс)
    /// </summary>
    public Tuple<Drink, float, float> GetRandomDrink()
    {
        return RandomUtils.SelectByWeight(drinksData, x => x.Item3);
    }
    
}
