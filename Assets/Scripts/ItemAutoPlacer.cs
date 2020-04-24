using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Items.MonoItems;

[RequireComponent(typeof(ItemPlace))]
public class ItemAutoPlacer : MonoBehaviour
{
    [SerializeField] private MonoItem itemToPlace;

    private void Awake()
    {
        if (itemToPlace != null)
            GetComponent<ItemPlace>().PlaceItem(itemToPlace);
    }
}
