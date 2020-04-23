using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


using Items;
/// <summary>
/// Класс стола, который может хранить в себе предметы
/// </summary>
public class Table : MonoBehaviour
{
    /// <summary>
    /// Места для предметов
    /// </summary>
    [SerializeField] private List<ItemPlace> places;
    

    

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
            freePlace.AddItem(item);
        }
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

            place.First().AddItem(item);
        }
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

}

/// <summary>
/// Место для установки предмета
/// </summary>
public class ItemPlace : MonoBehaviour
{
    /// <summary>
    /// Текущий предмет
    /// </summary>
    public IItem CurrentItem
    {
        get
        {
            if (currentItem == null)
                return new NullItem();

            return currentItem;
        }
        set
        {
            if(value == null)
            {
                currentItem = new NullItem();  
            }

            currentItem = value;
        }
    }
    [SerializeField] private IItem currentItem;

    /// <summary>
    /// Позиция в мировом пространстве
    /// </summary>
    public Vector3 Position { get => transform.position; }

    public void AddItem(IItem item)
    {
        var monoItem = item as MonoItem;

        // Если это предмет наследованный от MonoBehaviour
        if(monoItem != null)
        {
            monoItem.transform.parent = transform;
            monoItem.transform.position = Position;
            CurrentItem = monoItem;
        }
        else
        {
            CurrentItem = item;
        }
    }

    public IItem RemoveItem()
    {
        var item = CurrentItem;
        var monoItem = item as MonoItem;

        // Если это предмет наследованный от MonoBehaviour
        if (monoItem != null)
        {
            monoItem.transform.parent = null;
        }

        CurrentItem = null;
        return item;
    }
}
