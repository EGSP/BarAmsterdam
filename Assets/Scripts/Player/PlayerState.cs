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
            updateData.cursor = Player.TableCursor;
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
            
            
            updateData.hor = (int)DeviceInput.GetHorizontalAxis();
            updateData.ver = (int)DeviceInput.GetVerticalAxis();
            updateData.horDown = (int)DeviceInput.GetHorizontalAxisDown();
            updateData.verDown = (int)DeviceInput.GetVerticalAxisDown();
            
            return Move(updateData);
        }

        /// <summary>
        /// Высвобождение ресурсов
        /// </summary>
        public abstract void Dispose();

    }

    public class UpdateData
    {
        public float deltaTime;
        
        public TableTopCursor cursor;
        
        public int hor;
        public int ver;
        
        public int horDown;
        public int verDown;
    }
}
