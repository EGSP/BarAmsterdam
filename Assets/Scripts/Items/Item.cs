using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Player;
using Player.PlayerStates;

namespace Items
{
    /// <summary>
    /// ����� ��������
    /// </summary>
    public abstract class MonoItem : MonoBehaviour, IItem
    {
        /// <summary>
        /// ������������� ��������
        /// </summary>
        public string ID { get => id; }
        [SerializeField] private string id;
        

        /// <summary>
        /// ��������� ������ ��������� ��� ������, ��������� ���� ����������
        /// </summary>
        public abstract PlayerState GetPlayerState(PlayerController playerController);
    }

    /// <summary>
    /// ������� ������, ������������ ������� ���������
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
    /// ��������� ������������ �������
    /// </summary>
    public interface IItem : IPlayerStateChanger
    {
        /// <summary>
        /// ������������� ��������
        /// </summary>
        string ID { get; }
    }
}