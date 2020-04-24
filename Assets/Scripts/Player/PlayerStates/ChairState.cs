using UnityEngine;

using Core;
using Interiors;

namespace Player.PlayerStates
{
    public class ChairState : PlayerState
    {
        public ChairState(PlayerController player) : base(player)
        {

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

            var pos = Player.transform.position + new Vector3(Player.MoveStep * hor, Player.MoveStep * ver * Player.VerticalStepModifier, 0);
            
            // Ищем объект перед нами
            var interior = Player.GetComponentByLinecast<Interior>(pos);
            
            //Сесть на стул
            if (interior != null)
            {
                Player.interior = interior;
                interior.GetComponent<BoxCollider2D>().enabled = false;
                Player.transform.position = pos;
                return this;
            }
            //Не садиться на стул
            else if (ver + hor != 0  && Player.interior == null)
            {
                return new BaseState(Player);
            }
            // Встать со стула
            else if (ver != 0 && hor == 0)
            {
                Player.transform.position = pos;
                if (Player.interior != null)
                {
                    Player.interior.GetComponent<BoxCollider2D>().enabled = true;
                    Player.interior = null;
                }
                return new BaseState(Player);
            }

            return this;
        }
    }
}