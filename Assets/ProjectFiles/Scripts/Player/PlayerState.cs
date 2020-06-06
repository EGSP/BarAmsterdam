using System.Collections;
using System.Collections.Generic;
using System;

using Core;

using Player.Controllers;
using Player.PlayerCursors;

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
        /// Пробуждает состояние
        /// </summary>
        public abstract PlayerState Awake();

        public abstract PlayerState Move(UpdateData updateData);
        public abstract PlayerState Handle(UpdateData updateData);
        public abstract PlayerState Action(UpdateData updateData);
        public abstract PlayerState Extra(UpdateData updateData);
        
        /// <summary>
        /// Обновление состояния 
        /// </summary>
        /// <param name="updateData">Данные для обновления</param>
        public PlayerState UpdateState(UpdateData updateData)
        {
            // Нажатие на кнопку Z
            if (DeviceInput.GetHandleButtonDown())
            {
                return Handle(updateData);
            }

            // Нажатие на кнопку X
            if (DeviceInput.GetActionButtonDown())
            {
                return Action(updateData);
            }
            
            // Нажатие на Space
            if (DeviceInput.GetExtraButtonDown())
            {
                return Extra(updateData);
            }
            
            
            updateData.HorizontalAxisInput = (int)DeviceInput.GetHorizontalAxis();
            updateData.VerticalAxisInput = (int)DeviceInput.GetVerticalAxis();
            updateData.HorizontalAxisDownInput = (int)DeviceInput.GetHorizontalAxisDown();
            updateData.VerticalAxisDownInput = (int)DeviceInput.GetVerticalAxisDown();
            
            return Move(updateData);
        }

        /// <summary>
        /// Высвобождение ресурсов
        /// </summary>
        public abstract void Dispose();


    }

    public class UpdateData
    {
        public float DeltaTime;
        
        public int HorizontalAxisInput;
        public int VerticalAxisInput;
        
        public int HorizontalAxisDownInput;
        public int VerticalAxisDownInput;
    }
}
