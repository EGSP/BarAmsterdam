﻿using UnityEngine;

using Core;
using Interiors;

namespace Player.PlayerStates
{
    /// <summary>
    /// Стартовое поведение персонажа (ходьба, взаимодействие и т.д.)
    /// </summary>
    public class BaseState : PlayerState
    {
        public BaseState(PlayerController player) : base(player)
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
            var horDown = (int)DeviceInput.GetHorizontalAxisDown();
            var verDown = (int)DeviceInput.GetVerticalAxisDown();
            
            // Удерживание кнопки
            var hor = (int)DeviceInput.GetHorizontalAxis();
            var ver = (int)DeviceInput.GetVerticalAxis();

            

            if (DeviceInput.GetHandleButtonDown())
            {
                var pos = Vector3.zero;

                // Добавить ориентацию в игрока и по ней лайнкастить
            }

            // Если движемся или двигались только в одну сторону или только нажали
            if ((Mathf.Abs(hor) + Mathf.Abs(ver)) == 1)
            {
                var pos = Vector3.zero;

                if(hor != 0)
                {
                    // Это означает первое нажатие
                    if(hor == horDown)
                    {
                        Player.ChangeOrientation(hor, ver);

                        pos = Player.transform.position + Player.transform.right * Player.MoveStep * horDown;
                        // Ищем объект перед нами
                        var interior = Player.GetComponentByLinecast<Interior>(pos);
                        if (interior != null)
                        {
                            return interior.GetPlayerState(Player);
                        }
                    }
                }
                else if(ver != 0)
                {
                    // Это означает первое нажатие
                    if (ver == verDown)
                    {
                        Player.ChangeOrientation(hor, ver);

                        pos = Player.transform.position + Player.transform.up * Player.MoveStep * verDown * Player.VerticalStepModifier;
                        // Ищем объект перед нами
                        var interior = Player.GetComponentByLinecast<Interior>(pos);
                        if (interior != null)
                        {
                            return interior.GetPlayerState(Player);
                        }
                    }
                }
            }

            // Если персонаж должен сдвинутся и он стоит на месте
            if ((hor != 0 || ver != 0) && Player.IsMoving == false)
            {
                Player.Move(hor, ver);
            }

            return this;
        }
    }
}
