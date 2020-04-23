using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Player;
using Player.PlayerStates;

namespace Items
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

    /// <summary>
    /// Нулевой объект, возвращающий базовое состояние
    /// </summary>
    public class NullItem : IItem
    {
        public string ID => "NullItem";

        public PlayerState GetPlayerState(PlayerController playerController)
        {
            return new BaseState(playerController);
        }
    }

    /// <summary>
    /// Интерфейс определяющий предмет
    /// </summary>
    public interface IItem : IPlayerStateChanger
    {
        /// <summary>
        /// Идентификатор предмета
        /// </summary>
        string ID { get; }
    }
}