using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace Player.PlayerStates
{
    public abstract class PlayerState : IDisposable
    {
        /// <summary>
        /// Игрок этого состояния
        /// </summary>
        public readonly PlayerController Player;

        public PlayerState(PlayerController player)
        {
            if (player == null)
                throw new System.NullReferenceException();

            Player = player;
        }

        /// <summary>
        /// Обновление состояния 
        /// </summary>
        /// <param name="updateData">Данные для обновления</param>
        public abstract PlayerState UpdateState(UpdateData updateData);
        
        /// <summary>
        /// Высвобождение ресурсов
        /// </summary>
        public abstract void Dispose();

    }

    public class UpdateData
    {
        public float deltaTime;
    }

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
            


            // Если движемся или двигались только в одну сторону или только нажали
            if ((Mathf.Abs(hor) + Mathf.Abs(ver)) == 1)
            {
                var pos = Vector3.zero;

                if(hor != 0)
                {
                    // Это означает первое нажатие
                    if(hor == horDown)
                    {
                        pos =Player.transform.position + Player.transform.right * Player.MoveStep * horDown;
                        // Ищем объект перед нами
                        var interior = Player.GetComponentByLinecast<Interior>(pos);
                        if (interior != null)
                        {
                            Debug.Log(interior.ID);

                            return this;
                        }
                    }
                }
                else if(ver != 0)
                {
                    // Это означает первое нажатие
                    if (ver == verDown)
                    {
                        pos = Player.transform.position + Player.transform.up * Player.MoveStep * verDown;
                        // Ищем объект перед нами
                        var interior = Player.GetComponentByLinecast<Interior>(pos);
                        if (interior != null)
                        {
                            Debug.Log(interior.ID);

                            return this;
                        }
                    }
                }
            }

            // Если персонаж должен сдвинутся и он стоит на месте
            if ((hor != 0 || ver != 0) && Player.IsMoving == false)
                Player.Move(hor, ver);

            return this;
        }
    }
}
