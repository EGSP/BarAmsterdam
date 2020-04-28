using System.Collections;
using System.Collections.Generic;
using Player;
using Player.PlayerStates;
using Player.Controllers;
using UnityEngine;

namespace Interiors
{


    public class Chair : Interior
    {
        [SerializeField] private bool AutoOrientation = true;


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
                    ver = vertical;
                    hor = horisontal;
                    return;
                }
            };
            ver = 0;
            hor = 0;
            return;
        }

        public void Awake()
        {
            if(AutoOrientation)
                SetOrientation();
        }
    }
}
