using System;
using System.Collections.Generic;
using Interiors;
using UnityEngine;
using World;

namespace Bots.Behaviours
{
    public class CustomerBehaviour: AiBehaviour, ICustomerBehaviour, IMoveableBehaviour
    {
        /// <summary>
        /// Бар, к которому привязан бот
        /// </summary>
        public Bar Bar { get; set; }
        
        /// <summary>
        /// Запрошенный стул
        /// </summary>
        public Chair RequestedChair { get; set; }

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public Transform Transform => transform;
        public float MoveTime { get => moveSpeed; set=>moveSpeed = value; }
        [SerializeField] private float moveSpeed;
        
        /// <summary>
        /// Путь куда-либо
        /// </summary>
        public List<Vector3> Path { get; set; }
        
        


        public void OnEnable()
        {
            Debug.Log("Бот покупатель появился!");
        }

        public void Dispose()
        {
            DestroyBehaviour();
        }
    }
}