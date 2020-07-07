using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Player;
using Player.PlayerStates;
using Player.Controllers;
using UnityEngine;
using UnityEngine.Serialization;
using World;

namespace Interiors
{
    public class Chair : Interior, IHaveOrientation
    {
        [FormerlySerializedAs("AutoOrientation")] [SerializeField] private bool autoOrientation = true;

        /// <summary>
        /// Сделал ли стул установку
        /// </summary>
        public bool IsSetuped { get; private set; }
        /// <summary>
        /// Запрошен ли стул кем-то
        /// </summary>
        public bool Requested { get; set; }
        
        /// <summary>
        /// Серийный идентификатор или просто номер стула
        /// </summary>
        public int SerialId { get; private set; }
        
        public Orientation Orientation { get; set; }
        
        /// <summary>
        /// Стол, к которому привязан стул
        /// </summary>
        public TableTop table;

        /// <summary>
        /// Привязан ли стул к столу
        /// </summary>
        public bool HasTable => table == null;

        /// <summary>
        /// Вызывается методом KickSitting, когда нужно выгнать сидящего
        /// </summary>
        public event Action OnKickSitting = delegate {  };
        
        public override PlayerState GetPlayerState(PlayerController playerController)
        {
            return new ChairState(playerController, this);
        }

        /// <summary>
        /// Установка ориентации стула в зависимости от наличия стола
        /// </summary>
        public void Setup()
        {
            Orientation = new Orientation();
            
            var orientationList = new List<(int, int)>
            {
                (1, 0),
                (-1, 0),
                (0, 1),
                (0, -1)
            };
            
            foreach ((int, int) direction in orientationList)
            {
                var horizontal = direction.Item1 * SceneGrid.Instance.HorizontalModifier;
                var vertical = direction.Item2 * SceneGrid.Instance.VerticalModifier;
               
                // Проверка сферическая, поэтому может зацепить ненужные объекты.
                // Лучше делать центр объекта в центре ячейки
                var collider2D = Physics2D.OverlapCircle(
                    transform.position + new Vector3(horizontal, vertical),
                    0.1f);
                
                if (collider2D != null)
                {
                    var tableTop = collider2D.GetComponent<TableTop>();

                    if (tableTop != null)
                    {
                        table = tableTop;
                        Orientation.SetDirection(horizontal,vertical);
                        IsSetuped = true;
                        Debug.Log($"{horizontal} : {vertical}");
                        Debug.Log(Orientation.Direction);
                        return;
                    }
                }
            };
            
            if(table == null)
                Debug.Log($"Для стула {gameObject.name} не найдено стола");
            
            Orientation.SetDirection(1,0);
            IsSetuped = true;
            return;
        }

        public void Awake()
        {
            if(autoOrientation && IsSetuped == false)
                Setup();
        }

        /// <summary>
        /// Выгнать сидящего
        /// </summary>
        public void KickSitting()
        {
            OnKickSitting();
        }

        public void SetSerialId(int newSerialId)
        {
            SerialId = newSerialId;
        }
    }
}
