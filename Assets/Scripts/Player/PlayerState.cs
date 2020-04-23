using System.Collections;
using System.Collections.Generic;
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
}
