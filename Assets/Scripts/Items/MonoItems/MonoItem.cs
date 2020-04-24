using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.PlayerStates;
using Player.Controllers;

namespace Items.MonoItems
{
    /// <summary>
    /// Класс предмета
    /// </summary>
    public abstract class MonoItem : MonoBehaviour, IItem
    {
        /// <summary>
        /// Идентификатор предмета
        /// </summary>
        public string ID { get => id; }
        [SerializeField] private string id;
        

        /// <summary>
        /// Получение нового состояния для игрока, требуемое этим интерьером
        /// </summary>
        public abstract PlayerState GetPlayerState(PlayerController playerController);
    }
}