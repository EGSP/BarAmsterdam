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
            
            var bottle = actionItem as Bottle;
            if (bottle != null)
            {
                bottle.FillGlass(Glass);
            }
            else
            {
                NoIteractItemWarning(typeof(Bottle));
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
