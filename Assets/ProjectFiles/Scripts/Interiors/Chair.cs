using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Player;
using Player.PlayerStates;
using Player.Controllers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Interiors
{
    public class Chair : Interior, IHaveOrientation
    {
        [FormerlySerializedAs("AutoOrientation")] [SerializeField] private bool autoOrientation = true;

        /// <summary>
        /// Запрошен ли стул кем-то
        /// </summary>
        public bool Requested { get; set; }
        
        /// <summary>
        /// Серийный идентификатор или просто номер стула
        /// </summary>
        public int SerialId { get; private set; }
        
        public Orientation Orientation { get; set; }
        
        public TableTop table;

        /// <summary>
        /// Вызывается методом KickSitting, когда нужно выгнать сидящего
        /// </summary>
        public event Action OnKickSitting = delegate {  };
        
        public override PlayerState GetPlayerState(PlayerController playerController)
        {
            return new ChairState(playerController, this);
        }

        private void SetOrientation()
        {
            Orientation = new Orientation();
            
            var orientationList = new List<(int, int)>
            {
                (1, 0),
                (-1, 0),
                (0, 1),
                (0, -1)
            };
            
            foreach ((int, int) orientation in orientationList)
            {
                var vertical = orientation.Item1;
                var horizontal = orientation.Item2;
                
                Vector3 endPosition = new Vector3(transform.position.x + horizontal, transform.position.y + vertical, transform.position.z);
                
                GetComponent<BoxCollider2D>().enabled = false;
                var hit = Physics2D.Linecast(transform.position, endPosition, ~0);
                GetComponent<BoxCollider2D>().enabled = true;
                
                if (hit.collider != null)
                {
                    table = hit.collider.gameObject.GetComponent<TableTop>();
                    
                    Orientation.SetDirection(horizontal,vertical);
                    return;
                }
            };
            
            Orientation.SetDirection(1,0);
            return;
        }

        public void Start()
        {
            
            if(autoOrientation)
                SetOrientation();
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
