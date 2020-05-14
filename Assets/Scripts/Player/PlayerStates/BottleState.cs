using UnityEngine;

using Core;
using Interiors;
using Items.MonoItems;

using Player.Controllers;

namespace Player.PlayerStates
{
    public class BottleState : WithItemState
    {
        private Bottle Bottle;
        public BottleState(PlayerController player, Bottle bottle) : base(player)
        {
            item = bottle;
            Bottle = bottle;
        }

        public override PlayerState Action(UpdateData updateData)
        {
            var cursor = updateData.cursor;
            var tableTop = Player.GetComponentByLinecast<TableTop>(
                Player.transform.position + Player.ModifiedOrientation);

            MonoItem actionItem;
            if(cursor.IsActive)
                actionItem = (MonoItem) tableTop.TakeItemByReference(cursor.getItem(), false);
            else
            {
                actionItem = tableTop.TakeItemByDistance(Player.transform.position, false);
            }

            if (actionItem is Glass)
            {
                Glass glass = (Glass) actionItem;
                Bottle.PourOff(glass);
            }
            return this;
        }

        // Нечего высввобождать
        public override void Dispose()
        {
            return;
        }
    }
}