using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScaler : MonoBehaviour
{
    
    [SerializeField]private SpriteRenderer render;

    private void Awake()
    {
        if (render == null)
            render = GetComponent<SpriteRenderer>();

        // Order
        render.sortingOrder = (int)(-transform.position.y * 2);

        // Scale
        var scale = 1 - transform.position.y * 0.2f;
        transform.localScale = new Vector3(scale, scale, 1);
    }
}
