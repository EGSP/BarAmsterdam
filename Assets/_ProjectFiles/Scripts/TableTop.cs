using System.Collections;
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
    /// Существует ли свободное место для предмета
    /// </summary>
    public virtual bool PlaceAvailable
    {
        get
        {
            var freePlace = places.FirstOrDefault(x => x.CurrentItem.ID == "NullItem");

            // Если нашли свободное место
            if(freePlace != null)
            {
                return true;
            }

            return false;
        }
    }
    
    /// <summary>
    /// Логика добавления предмета
    /// </summary>
    /// <param name="place">Место куда должен быть сложен предмет</param>
    /// <param name="itemToAdd">Добавляемый предмет</param>
    protected virtual void AddItem(ItemPlace place, IItem itemToAdd)
    {
        if (place != null)
        {
            place.PlaceItem(itemToAdd);

            OnCollectionChanged();
        }
    }

    /// <summary>
    /// Добавляет предмет на стол в любую свободную ячейку
    /// </summary>
    /// <param name="item">Добавляемый предмет</param>
    public virtual void AddItemToFreePlace(IItem item)
    {
        var freePlace = places.FirstOrDefault(x => x.CurrentItem.ID == "NullItem");

        AddItem(freePlace,item);
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
            var place = freePlaces.OrderBy(x =>
                (x.Position - initiatorPosition).sqrMagnitude);

            AddItem(place.First(),item);
        }
        else
        {
            AddItem(null,item);
        }
    }

    /// <summary>
    /// Логика убирания предмета
    /// </summary>
    protected virtual IItem TakeItem(ItemPlace place)
    {
        return place.RemoveItem();
    }

    /// <summary>
    /// Возвращает ближайший предмет со стола и удаляет из ячейки. Может вернуть NullItem
    /// </summary>
    public virtual IItem TakeItemByDistance(Vector3 initiatorPosition)
    {
        // Вычисляем monoItem
        var takeablePlaces = places.Where(x => x.CurrentItem as MonoItem != null);
        if (takeablePlaces.Count() > 0)
        {
            var orderedPlaces = takeablePlaces.OrderBy(x
                => (x.Position - initiatorPosition).sqrMagnitude);
            var item = TakeItem(orderedPlaces.First());

            return item;
        }

        return new NullItem();
        
        // Ошибку можно было бы вернуть, если бы мы могли узнать про существование MonoItems на столе
        // throw new Exception("There is no MonoItem on this TableTop");
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
            return TakeItem(coincidence);

        return new NullItem();
    }

    /// <summary>
    /// Возвращает предмет указывающий на тот же объект, что и item. Может вернуть null.
    /// Возвращенный предмет удаляется со стола
    /// </summary>
    public T TakeItemByReference<T>(IItem item) where T: class
    {
        var coincidence = places.FirstOrDefault(x => x.CurrentItem == item);

        if (coincidence != null)
            return TakeItem(coincidence) as T;

        return null;
    }

    /// <summary>
    /// Получение ссылки на предмет
    /// </summary>
    protected virtual IItem PopTakeableItem(ItemPlace place)
    {
        return place.CurrentItem;
    }

    /// <summary>
    /// Возвращает ссылку на ближайший предмет. Может вернуть NullItem
    /// </summary>
    public IItem PopTakeableItemByDistance(Vector3 initiatorPosition)
    {
        // Вычисляем monoItem
        var takeablePlaces = places.Where(x => x.CurrentItem as MonoItem != null);
        if (takeablePlaces.Count() > 0)
        {
            var orderedPlaces = takeablePlaces.OrderBy(x 
                => (x.Position - initiatorPosition).sqrMagnitude);
            var item = PopTakeableItem(orderedPlaces.First());

            return item;
        }

        return new NullItem();
    }

    /// <summary>
    /// Возвращает предмет указывающий на тот же объект, что и item. Может вернуть NullItem.
    /// Возвращенный предмет НЕ удаляется со стола
    /// </summary>
    /// <param name="item">Ссылка на нужный объект</param>
    public IItem PopItemByReference(IItem item)
    {
        var coincidence = places.FirstOrDefault(x => x.CurrentItem == item);

        if (coincidence != null)
            return PopTakeableItem(coincidence);

        return new NullItem();
    }

    /// <summary>
    /// Возвращает ссылку на найденый по идентификатору предмет. Может вернуть null.
    /// </summary>
    /// <param name="itemId"></param>
    public IItem FindItemById(string itemId)
    {
        var coincidence = places.FirstOrDefault(x => x.CurrentItem.ID == itemId);

        if (coincidence != null)
            return PopTakeableItem(coincidence);

        return null;
    }

    /// <summary>
    /// Поиск предмета по выражению и типу. Может вернуть null, если предмет не найден
    /// </summary>
    public T FindItemByExpression<T>(Func<T, bool> expression) where T: class
    {
        for (var i = 0; i < places.Count; i++)
        {
            if (places[i].CurrentItem != null)
            {
                var convertedItem = places[i].CurrentItem as T;
                if (convertedItem != null)
                {
                    var success = expression(convertedItem);

                    if (success)
                        return convertedItem;
                    
                }
            }
        }

        return null;
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

    /// <summary>
    /// Проверка совместимости типа предмета со столом
    /// </summary>
    /// <returns></returns>
    public virtual bool TypeCompatibility(IItem item)
    {
        return true;
    }

    /// <summary>
    /// Можно ли ставить предмет на стол
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Available(IItem item)
    {
        return PlaceAvailable & TypeCompatibility(item);
    }


    private void OnDrawGizmos()
    {
        if (places == null)
            return;
        
        if (places.Count == 0)
            return;
        
        for(var i = 0; i < places.Count; i++)
        {
            var place = places[i];

            if (place != null)
                Gizmos.DrawIcon(place.transform.position, "ItemPlaceIcon",true);
        }
    }
}
