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
            var cursor = Player.TableCursor;
            var tableTop = Player.GetComponentByLinecast<TableTop>(
                Player.transform.position + Player.ModifiedOrientation);

            if (tableTop == null)
            {
                NoTableTopWarning();
                return this;
            }

            MonoItem actionItem;
            if (cursor.IsActive)
            {
                actionItem = tableTop.PopItemByReference(cursor.GetSelectedItem()) as MonoItem;
            }
            else
            {
                actionItem = tableTop.PopTakeableItemByDistance(Player.transform.position) as MonoItem;
            }

            var glass = actionItem as Glass;
            if (glass != null)
            {
                Bottle.FillGlass(glass);
            }
            else
            {
                NoIteractItemWarning(typeof(Glass));
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