using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс объекта окружения
/// </summary>
public abstract class Interior : MonoBehaviour
{
    /// <summary>
    /// Идентификатор объекта окружения
    /// </summary>
    public string ID { get=>id; }
    [SerializeField] private string id;
}
