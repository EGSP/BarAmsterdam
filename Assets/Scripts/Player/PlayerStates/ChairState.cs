using UnityEngine;

using Core;
using Interiors;

using Player.Controllers;

namespace Player.PlayerStates
{
    public class ChairState : PlayerState
    {
        public Chair Chair;
        public ChairState(PlayerController player, Chair interior) : base(player)
        {
            Chair = interior;
            Player.StopMovement();
            Player.transform.position = Chair.transform.position;
            Player.SpriteRenderer.flipX = Chair.horOrientation > 0 ? true : false;
            Player.Animator.Play("SitDown");

        }

        // Нечего высввобождать
        public override void Dispose()
        {
            return;
        }
        
        public override PlayerState Action(UpdateData updateData)
        {
            Debug.Log("chair action");
            return this;
        }
        
        public override PlayerState Handle(UpdateData updateData)
        {
            Debug.Log("chair hdl");
            return this;
        }
        
        public override PlayerState Extra(UpdateData updateData)
        {
            Debug.Log("chair xtra");
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
                if (hor * Chair.horOrientation + ver * Chair.verOrientation == 0)
                {
                    Player.MoveWithoutCollision(hor, ver);
                    
                    if (hor > 0)
                    {
                        Player.SpriteRenderer.flipX = false;
                        Player.Animator.Play("MoveRight");
                    }
                        
                    else if (hor < 0)
                    {
                        Player.SpriteRenderer.flipX = true;
                        Player.Animator.Play("MoveRight");
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