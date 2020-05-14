using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Items.MonoItems;

public static class FoodInfo
{
    public static Dictionary<Bottle.Drink, float> BottlePrice = new Dictionary<Bottle.Drink, float>();
    public static Dictionary<Bottle.Drink, int>   BottleVolume = new Dictionary<Bottle.Drink, int>();
    public static Dictionary<Bottle.Drink, float> BottleWaitingTime = new Dictionary<Bottle.Drink, float>();
    public static Dictionary<Bottle.Drink, float> BottleEatingTime = new Dictionary<Bottle.Drink, float>();
    public static Dictionary<Bottle.Drink, float> BottleOrderChance = new Dictionary<Bottle.Drink, float>();
        
    public static Dictionary<Bottle.Drink, float> GlassPrice = new Dictionary<Bottle.Drink, float>();
    public static Dictionary<Bottle.Drink, float> GlassWaitingTime = new Dictionary<Bottle.Drink, float>();
    public static Dictionary<Bottle.Drink, float> GlassEatingTime = new Dictionary<Bottle.Drink, float>();
    public static Dictionary<Bottle.Drink, float> GlassOrderChance = new Dictionary<Bottle.Drink, float>();

    static FoodInfo()
    { 
        BottlePrice = new Dictionary<Bottle.Drink, float>();
        BottleVolume = new Dictionary<Bottle.Drink, int>();
        BottleWaitingTime = new Dictionary<Bottle.Drink, float>();
        BottleEatingTime = new Dictionary<Bottle.Drink, float>();
        BottleOrderChance = new Dictionary<Bottle.Drink, float>();
        
        GlassPrice = new Dictionary<Bottle.Drink, float>();
        GlassWaitingTime = new Dictionary<Bottle.Drink, float>();
        GlassEatingTime = new Dictionary<Bottle.Drink, float>();
        GlassOrderChance = new Dictionary<Bottle.Drink, float>();
    
        BottlePrice.Add(Bottle.Drink.Whisky,        2.0f);
        BottlePrice.Add(Bottle.Drink.Vodka,         1.5f);
        BottlePrice.Add(Bottle.Drink.Gin,           1.5f);
        BottlePrice.Add(Bottle.Drink.Brandy,        2.0f);
        BottlePrice.Add(Bottle.Drink.Wine,          1.4f);
        BottlePrice.Add(Bottle.Drink.Beer,          0.4f);
    
        BottleVolume.Add(Bottle.Drink.Whisky,       5);
        BottleVolume.Add(Bottle.Drink.Vodka,        5);
        BottleVolume.Add(Bottle.Drink.Gin,          5);
        BottleVolume.Add(Bottle.Drink.Brandy,       5);
        BottleVolume.Add(Bottle.Drink.Wine,         3);
        BottleVolume.Add(Bottle.Drink.Beer,         2);
    
        BottleWaitingTime.Add(Bottle.Drink.Whisky,  60.0f);
        BottleWaitingTime.Add(Bottle.Drink.Vodka,   60.0f);
        BottleWaitingTime.Add(Bottle.Drink.Gin,     60.0f);
        BottleWaitingTime.Add(Bottle.Drink.Brandy,  60.0f);
        BottleWaitingTime.Add(Bottle.Drink.Wine,    60.0f);
        BottleWaitingTime.Add(Bottle.Drink.Beer,    45.0f);
    
        BottleEatingTime.Add(Bottle.Drink.Whisky,  210.0f);
        BottleEatingTime.Add(Bottle.Drink.Vodka,   180.0f);
        BottleEatingTime.Add(Bottle.Drink.Gin,     180.0f);
        BottleEatingTime.Add(Bottle.Drink.Brandy,  210.0f);
        BottleEatingTime.Add(Bottle.Drink.Wine,    150.0f);
        BottleEatingTime.Add(Bottle.Drink.Beer,    132.0f);
        
        BottleOrderChance.Add(Bottle.Drink.Whisky, 0.041f);
        BottleOrderChance.Add(Bottle.Drink.Vodka,  0.041f);
        BottleOrderChance.Add(Bottle.Drink.Gin,    0.041f);
        BottleOrderChance.Add(Bottle.Drink.Brandy, 0.041f);
        BottleOrderChance.Add(Bottle.Drink.Wine,   0.041f);
        BottleOrderChance.Add(Bottle.Drink.Beer,   0.041f);
        
        GlassPrice.Add(Bottle.Drink.Whisky,       0.4f);
        GlassPrice.Add(Bottle.Drink.Vodka,        0.3f);
        GlassPrice.Add(Bottle.Drink.Gin,          0.3f);
        GlassPrice.Add(Bottle.Drink.Brandy,       0.4f);
        GlassPrice.Add(Bottle.Drink.Wine,         0.35f);
        GlassPrice.Add(Bottle.Drink.Beer,         0.2f);
    
        GlassWaitingTime.Add(Bottle.Drink.Whisky, 30.0f);
        GlassWaitingTime.Add(Bottle.Drink.Vodka,  30.0f);
        GlassWaitingTime.Add(Bottle.Drink.Gin,    30.0f);
        GlassWaitingTime.Add(Bottle.Drink.Brandy, 30.0f);
        GlassWaitingTime.Add(Bottle.Drink.Wine,   30.0f);
        GlassWaitingTime.Add(Bottle.Drink.Beer,   30.0f);
    
        GlassEatingTime.Add(Bottle.Drink.Whisky,  72.0f);
        GlassEatingTime.Add(Bottle.Drink.Vodka,   60.0f);
        GlassEatingTime.Add(Bottle.Drink.Gin,     60.0f);
        GlassEatingTime.Add(Bottle.Drink.Brandy,  60.0f);
        GlassEatingTime.Add(Bottle.Drink.Wine,    60.0f);
        GlassEatingTime.Add(Bottle.Drink.Beer,    72.0f);
        
        GlassOrderChance.Add(Bottle.Drink.Whisky, 0.125f);
        GlassOrderChance.Add(Bottle.Drink.Vodka,  0.125f);
        GlassOrderChance.Add(Bottle.Drink.Gin,    0.125f);
        GlassOrderChance.Add(Bottle.Drink.Brandy, 0.125f);
        GlassOrderChance.Add(Bottle.Drink.Wine,   0.125f);
        GlassOrderChance.Add(Bottle.Drink.Beer,   0.125f);
    }
    
}
