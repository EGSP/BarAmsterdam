using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScaler : MonoBehaviour
{
    
    [SerializeField]private SpriteRenderer render;
    [SerializeField]private bool isUpdate = false;


    private void Scale()
    {
        if (render == null)
            render = GetComponent<SpriteRenderer>();

        // Order
        render.sortingOrder = (int)(-transform.position.y * 2);

        // Scale
        var scale = 1 - transform.position.y * 0.2f;
        transform.localScale = new Vector3(scale, scale, 1);
    }
    private void Awake()
    {
        Scale();
    }

    private void Update()
    {
        if (isUpdate)
            Scale();
    }
}
