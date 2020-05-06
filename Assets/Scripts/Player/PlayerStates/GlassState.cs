using UnityEngine;

using Core;
using Interiors;
using Items.MonoItems;

using Player.Controllers;

namespace Player.PlayerStates
{
    public class GlassState : WithItemState
    {
        private Glass Glass;
        public GlassState(PlayerController player,Glass glass) : base(player)
        {
            item = glass;
            Glass = glass;
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

            if (actionItem is Bottle)
            {
                Bottle bottle = (Bottle) actionItem;
                bottle.PourOff(Glass);
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
