using System.Collections;
using System.Collections.Generic;
using Player;
using Player.PlayerStates;
using Player.Controllers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Interiors
{
    public class Chair : Interior
    {
        [FormerlySerializedAs("AutoOrientation")] [SerializeField] private bool autoOrientation = true;

        /// <summary>
        /// Ориентация по вертикали от -1 до 1
        /// </summary>
        public int verOrientation { get; private set; } = 0;

        /// <summary>
        /// Ориентация по горизонтали от -1 до 1
        /// </summary>
        public int horOrientation { get; private set; } = 1;

        public TableTop table;

        public override PlayerState GetPlayerState(PlayerController playerController)
        {
            return new ChairState(playerController, this);
        }

        private void SetOrientation()
        {
            List<(int, int)> OrientationList = new List<(int, int)>
            {
                (1, 0),
                (-1, 0),
                (0, 1),
                (0, -1)
            };
            
            foreach ((int, int) orientation in OrientationList)
            {
                var vertical = orientation.Item1;
                var horisontal = orientation.Item2;
                
                Vector3 endPosition = new Vector3(transform.position.x + horisontal, transform.position.y + vertical, transform.position.z);
                
                GetComponent<BoxCollider2D>().enabled = false;
                var hit = Physics2D.Linecast(transform.position, endPosition, ~0);
                GetComponent<BoxCollider2D>().enabled = true;
                
                if (hit.collider != null)
                {
                    table = hit.collider.gameObject.GetComponent<TableTop>();
                    verOrientation = vertical;
                    horOrientation = horisontal;
                    return;
                }
            };
            verOrientation = 0;
            horOrientation = 1;
            return;
        }

        public void Start()
        {
            if(autoOrientation)
                SetOrientation();
        }
    }
}
