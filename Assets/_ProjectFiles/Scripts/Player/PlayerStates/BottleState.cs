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
        public override MonoItem Item => Bottle;

        public BottleState(PlayerController player, Bottle bottle) : base(player)
        {
            Bottle = bottle;
        }

        public override PlayerState Handle(UpdateData updateData)
        {
            var tableTop = FindAcceptableObject<TableTop>();

            if (tableTop == null)
            {
                NoTableTopWarning();
                return this;
            }
            else
            {
                var drinkCabinet = tableTop as DrinkCabinet;
                if (drinkCabinet != null)
                {
                    if (drinkCabinet.CompareBottles(Bottle))
                    {
                        drinkCabinet.DestroyBottle(Bottle);
                        return new BaseState(Player);
                    }
                }
            }
            
            if (tableTop.Available(Item))
            {
                tableTop.AddItemToNearest(Item,Player.transform.position);
                return new BaseState(Player);
            }

            return this;
        }

        public override PlayerState Action(UpdateData updateData)
        {
            var cursor = Player.TableCursor;
            var tableTop = FindAcceptableObject<TableTop>();

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