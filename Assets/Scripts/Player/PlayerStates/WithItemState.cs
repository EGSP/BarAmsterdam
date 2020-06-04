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
            // Проверка можно ли сесть за стул (полный ли стол?)
            int horDown = updateData.HorizontalAxisDownInput;
            int verDown = updateData.VerticalAxisDownInput;

            // Если нажали на одну из кнопок
            if (horDown != 0 || verDown != 0)
            {
                Player.ChangeOrientation(horDown, verDown);
            }

            var pos = Vector3.zero;
            pos = Player.transform.position + Player.ModifiedOrientation;
            
            // Ищем объект перед нами
            var interior = Player.GetComponentByLinecast<Interior>(pos);
            if (interior != null)
            {
                if (interior is Chair)
                {
                    if ( !((Chair) interior).table.PlaceAvailable )
                    {
                        return this;
                    }
                }
            }
            
            PlayerState newState = base.Move(updateData);
            if (newState is BaseState)
            {
                return this;
            }
            // Если внутри BaseState изменилось состояние
            ChairState chairState = (ChairState) newState;
            var table = chairState.Chair.table;
            if (table.PlaceAvailable)
            {
                chairState.Chair.table.AddItem(item);
                return chairState;
            }

            return this;
        }

        public override PlayerState Handle(UpdateData updateData)
        {
            var tableTop = Player.GetComponentByLinecast<TableTop>(
                Player.transform.position + Player.ModifiedOrientation);

            if (tableTop == null)
            {
                NoTableTopWarning();
                return this;
            }

            if (tableTop.PlaceAvailable)
            {
                tableTop.AddItemToNearest(item,Player.transform.position);
                return new BaseState(Player);
            }

            return this;

        }

        public override PlayerState Extra(UpdateData updateData)
        {
            base.Extra(updateData);
            return this;
        }

        
    }
}