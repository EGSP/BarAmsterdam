using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Player;
using Player.PlayerStates;
using Player.Controllers;

namespace Interiors
{
    /// <summary>
    /// Класс объекта окружения
    /// </summary>
    public abstract class Interior : MonoBehaviour, IPlayerStateChanger
    {
        /// <summary>
        /// Идентификатор объекта окружения
        /// </summary>
        public string ID { get => id; }
        [SerializeField] private string id;


        /// <summary>
        /// Получение нового состояния для игрока, требуемое этим интерьером
        /// </summary>
        public abstract PlayerState GetPlayerState(PlayerController playerController);
    }
}