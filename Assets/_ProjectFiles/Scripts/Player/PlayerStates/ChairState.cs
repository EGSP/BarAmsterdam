using UnityEngine;

using Core;
using Interiors;

using Player.Controllers;
using World;

namespace Player.PlayerStates
{
    public class ChairState : PlayerState
    {
        public Chair Chair;
        public ChairState(PlayerController player, Chair interior) : base(player)
        {
            Chair = interior;
        }

        // Нечего высввобождать
        public override void Dispose()
        {
            return;
        }

        public override PlayerState Awake()
        {
            // Выкладываем предмет на стол если есть место
            var withItemState = Player.CurrentPlayerState as WithItemState;
            if (withItemState != null)
            {
                if (Chair.table.Available(withItemState.Item))
                {
                    Chair.table.AddItemToNearest(withItemState.Item, Chair.transform.position);
                }
                else
                {
                    // Возвращаем прежнее состояние если нельзя сесть
                    return withItemState;
                }
            }

            // Садимся
            Player.StopMovement();
            Player.transform.position = Chair.transform.position;
            Player.SpriteRenderer.flipX = Chair.Orientation.Direction.x > 0 ? true : false;
            Player.PlayAnimation("SitDown");
            return this;
        }

        public override PlayerState Action(UpdateData updateData)
        {
            Debug.Log("chair action");
            return this;
        }
        
        public override PlayerState Handle(UpdateData updateData)
        {
            Debug.Log("chair handle");
            return this;
        }
        
        public override PlayerState Extra(UpdateData updateData)
        {
            Debug.Log("chair extra");
            return this;
        }

        public override PlayerState Move(UpdateData updateData)
        {
            // Нажатие и удерживание могут совпадать (особенность движка)
            // Нажатие на кнопку
            var hor = (int)DeviceInput.GetHorizontalAxisDown();
            var ver = (int)DeviceInput.GetVerticalAxisDown();


            if (hor != 0 || ver != 0)
            {
                if (hor * Chair.Orientation.Direction.x + ver * Chair.Orientation.Direction.y == 0)
                {
                    Player.transform.position = SceneGrid.Instance
                        .GetNearestCellCentralized(Player.transform.position);
                    
                    Player.MoveWithoutCollision(hor, ver);
                    
                    if (hor > 0)
                    {
                        Player.SpriteRenderer.flipX = false;
                        Player.Animator.Play("MoveRight");
                    }
                    else if (hor < 0)
                    {
                        Player.SpriteRenderer.flipX = true;
                        Player.Animator.Play("MoveLeft");
                    }
                    else if (ver > 0)
                    {
                        Player.Animator.Play("MoveUp");
                    }
                    else if (ver < 0)
                    {
                        Player.Animator.Play("MoveDown");
                    }
                    
                    return new BaseState(Player);
                }
            }
            
            return this;
        }
    }
}