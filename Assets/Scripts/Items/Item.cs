using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public abstract class Item : MonoBehaviour
    {
        /// <summary>
        /// ������������� ��������
        /// </summary>
        public string ID { get => id; }
        [SerializeField] private string id;

    }
}