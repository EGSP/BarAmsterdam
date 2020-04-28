using UnityEngine;

using Core;
using Interiors;

using Player.Controllers;

namespace Player.PlayerStates
{
    public class ChairState : PlayerState
    {
        private Interior chair;
        public ChairState(PlayerController player, Interior interior) : base(player)
        {
            chair = interior;
            Player.StopMovement();
            Player.transform.position = chair.transform.position;
            Player.rend.flipX = chair.hor > 0 ? true : false;
            Player.animator.Play("SitDown");

        }

        // Нечего высввобождать
        public override void Dispose()
        {
            return;
        }

        public override PlayerState UpdateState(UpdateData updateData)
        {
            // Нажатие и удерживание могут совпадать (особенность движка)
            // Нажатие на кнопку
            var hor = (int)DeviceInput.GetHorizontalAxisDown();
            var ver = (int)DeviceInput.GetVerticalAxisDown();


            if (hor != 0 || ver != 0)
            {
                if (hor * chair.hor + ver * chair.ver == 0)
                {
                    Player.MoveWithoutCollision(hor, ver);
                    
                    if (hor > 0)
                    {
                        Player.rend.flipX = false;
                        Player.animator.Play("MoveRight");
                    }
                        
                    else if (hor < 0)
                    {
                        Player.rend.flipX = true;
                        Player.animator.Play("MoveRight");
                    }
                    else if (ver > 0)
                    {
                        Player.animator.Play("MoveUp");
                    }
                    else if (ver < 0)
                    {
                        Player.animator.Play("MoveDown");
                    }
                    
                    return new BaseState(Player);
                }
            }
            

            /// СДЕЛАТЬ КАК В BASESTATE (ГОРИЗОНТАЛЬ ПРИОРИТЕТНЕЕ ВЕРТИКАЛИ)

            //var pos = Player.transform.position + new Vector3(Player.MoveStep * hor, Player.MoveStep * ver * Player.VerticalStepModifier, 0);
            
            //// Ищем объект перед нами
            //var interior = Player.GetComponentByLinecast<Interior>(pos);
            
            /// ТЕПЕРЬ НАМ НЕ НУЖНО ИСКАТЬ СТУЛ, МЫ УЖЕ НА СТУЛЕ, Т.К. УЖЕ ВКЛЮЧИЛИ ЭТО СОСТОЯНИЕ

            ////Сесть на стул
            //if (interior != null)
            //{
            //    Player.interior = interior;
            //    interior.GetComponent<BoxCollider2D>().enabled = false;
            //    Player.transform.position = pos;
            //    return this;
            //}
            ////Не садиться на стул
            //else if (ver + hor != 0  && Player.interior == null)
            //{
            //    return new BaseState(Player);
            //}
            //// Встать со стула
            //else if (ver != 0 && hor == 0)
            //{
            //    Player.transform.position = pos;
            //    if (Player.interior != null)
            //    {
            //        Player.interior.GetComponent<BoxCollider2D>().enabled = true;
            //        Player.interior = null;
            //    }
            //    return new BaseState(Player);
            //}

            /// ВСТАТЬ СО СТУЛА МОЖНО ЕСЛИ ИГРОК НАЖАЛ ИЛИ УДЕРЖИВАЕТ КНОПКУ ОСИ
            /// НАПРАВЛЕНИЕ БЕРЕМ КАК В МЕТОДЕ MOVE В КОНТРОЛЛЕРЕ. ПОКА МОЖНО СКОПИПАСТИТЬ

            return this;
        }
    }
}