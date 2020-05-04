using UnityEngine;

using Core;
using Interiors;
using Items.MonoItems;

using Player.Controllers;

namespace Player.PlayerStates
{
    public class BottleState : WithItemState
    {
        public BottleState(PlayerController player, Bottle bottle) : base(player)
        {
            item = bottle;
            Debug.Log("btl");

        }

        public override PlayerState Action(UpdateData updateData)
        {
            Debug.Log("btl action");
            return this;
        }

        public override PlayerState Extra(UpdateData updateData)
        {
            Debug.Log("btl extra");
            return this;
        }

        // Нечего высввобождать
        public override void Dispose()
        {
            return;
        }
    }
}