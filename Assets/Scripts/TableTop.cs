﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


using Items;
using Items.MonoItems;
using Player.PlayerCursors;
using System;


/// <summary>
/// Класс стола, который может хранить в себе предметы
/// </summary>
public class TableTop : MonoBehaviour, ICursorEnumerable
{
    /// <summary>
    /// Места для предметов
    /// </summary>
    [SerializeField] private List<ItemPlace> places;

    /// <summary>
    /// Вызывается при изменении коллекции
    /// </summary>
    public event Action OnCollectionChanged = delegate { };




    /// <summary>
    /// Добавляет предмет на стол в любую свободную ячейку
    /// </summary>
    /// <param name="item">Добавляемый предмет</param>
    public void AddItem(IItem item)
    {
        var freePlace = places.FirstOrDefault(x => x.CurrentItem.ID == "NullItem");

        // Если нашли свободное место
        if(freePlace != null)
        {
            freePlace.PlaceItem(item);
        }

        OnCollectionChanged();
    }

    /// <summary>
    /// Добавляет предмет на стол в ближайшую свободную ячейку
    /// </summary>
    /// <param name="item">Добавляемый предмет</param>
    /// <param name="initiatorPosition">Позиция вызывающего эту функцию объекта</param>
    public void AddItemToNearest(IItem item,Vector3 initiatorPosition)
    {
        var freePlaces = places.Where(x => x.CurrentItem.ID == "NullItem");

        if (freePlaces.Count() > 0)
        {
            var place = freePlaces.OrderBy(x => (x.Position - initiatorPosition).sqrMagnitude);

            place.First().PlaceItem(item);
        }

        OnCollectionChanged();
    }

    /// <summary>
    /// Возвращает ближайший предмет со стола и удаляет из ячейки. Может вернуть NullItem
    /// </summary>
    /// <param name="initiatorPosition"></param>
    public IItem TakeItemByDistance(Vector3 initiatorPosition)
    {
        // Вычисляем monoItem
        var takeablePlaces = places.Where(x => x.CurrentItem as MonoItem != null);
        if (takeablePlaces.Count() > 0)
        {
            var orderedPlaces = takeablePlaces.OrderBy(x => (x.Position - initiatorPosition).sqrMagnitude);
            var item = orderedPlaces.First().RemoveItem();

            return item;
        }

        return new NullItem();
    }

    /// <summary>
    /// Возвращает предмет указывающий на тот же объект, что и item. Может вернуть NullItem.
    /// Возвращенный предмет удаляется со стола
    /// </summary>
    /// <param name="item">Ссылка на нужный объект</param>
    /// <returns></returns>
    public IItem TakeItemByReference(IItem item)
    {
        var coincidence = places.FirstOrDefault(x => x.CurrentItem == item);

        if (coincidence != null)
            return coincidence.RemoveItem();

        return new NullItem();
    }

    /// <summary>
    /// Возвращает ссылку на найденый по идентификатору предмет. Может вернуть NullItem.
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public IItem FindItemByID(string itemID)
    {
        var coincidence = places.FirstOrDefault(x => x.CurrentItem.ID == itemID);

        if (coincidence != null)
            return coincidence.RemoveItem();

        return new NullItem();
    }

    /// <summary>
    /// Получение коллекции текущих предметов
    /// </summary>
    /// <returns></returns>
    public List<MonoItem> GetCollection()
    {
        var list = places
            .Select(x => x.CurrentItem)
            .Where(x => x as MonoItem != null)
            .Cast<MonoItem>()
            .ToList();

        return list;
    }


    private void OnDrawGizmos()
    {
        for(int i = 0; i < places.Count; i++)
        {
            var place = places[i];

            if (place != null)
                Gizmos.DrawIcon(place.transform.position, "ItemPlaceIcon",true);
        }
    }
}