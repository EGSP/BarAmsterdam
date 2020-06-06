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

        /// <summary>
        /// Предмет состояния
        /// </summary>
        public virtual MonoItem Item { get; }
        
        // Нечего высввобождать
        public override void Dispose()
        {
            return;
        }
        
        public override PlayerState Handle(UpdateData updateData)
        {
            var tableTop = FindAcceptableObject<TableTop>();

            if (tableTop == null)
            {
                NoTableTopWarning();
                return this;
            }

            if (tableTop.Available(Item))
            {
                tableTop.AddItemToNearest(Item,Player.transform.position);
                return new BaseState(Player);
            }

            return this;

        }

        public override PlayerState Extra(UpdateData updateData)
        {
            // base.Extra(updateData);
            return this;
        }

        
    }
}