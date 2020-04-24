using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.PlayerStates;
using Player.Controllers;

namespace Items.MonoItems
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
}