using UnityEngine;

using Items;
using Items.MonoItems;
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

    /// <summary>
    /// Метод изменяет настройки Trasnform предмета (позицию, родителя)
    /// </summary>
    /// <param name="item"></param>
    public void PlaceItem(IItem item)
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
