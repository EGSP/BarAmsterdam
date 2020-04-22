using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс объекта окружения
/// </summary>
public abstract class Interior : MonoBehaviour
{
    private float scale;
    private SpriteRenderer render;
    
    /// <summary>
    /// Идентификатор объекта окружения
    /// </summary>
    public string ID { get=>id; }
    [SerializeField] private string id;

    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();

        // Order
        render.sortingOrder = (int)(-transform.position.y * 2);

        // Scale
        scale = 1 - transform.position.y * 0.2f;
        transform.localScale = new Vector3(scale, scale, 1);
    }
}
