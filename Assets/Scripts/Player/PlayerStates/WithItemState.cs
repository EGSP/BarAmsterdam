using UnityEngine;

using Core;
using Interiors;

using Player.Controllers;
using Player;
using Items.MonoItems;

namespace Player.PlayerStates
{
    public abstract class WithItemState : BaseState
    {
        public WithItemState(PlayerController player) : base(player)
        {

        }
        
        public MonoItem item;
        // Нечего высввобождать
        public override void Dispose()
        {
            return;
        }

        public override PlayerState Move(UpdateData updateData)
        {
            Debug.Log(string.Format("Move with {0}", item));
            PlayerState newState = base.Move(updateData);
            if (newState is BaseState)
            {
                return this;
            }
            // Если внутри BaseState изменилось состояние
            ChairState chairState = (ChairState) newState;
            chairState.Chair.table.AddItem(item);
            return chairState;
        }

        public override PlayerState Handle(UpdateData updateData)
        {
            Debug.Log(string.Format("Handle {0}", item));
            var tableTop = Player.GetComponentByLinecast<TableTop>(
                Player.transform.position + Player.ModifiedOrientation);

            tableTop.AddItem(item);
            return new BaseState(Player);
        }
    }
}