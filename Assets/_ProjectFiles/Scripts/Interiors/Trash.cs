using Items;
using Items.MonoItems;
using UnityEngine;

namespace Interiors
{
    public class Trash: TableTop
    {
        public override bool PlaceAvailable { get => true; }

        public override bool TypeCompatibility(IItem item)
        {
            var bottle = item as Bottle;
            
            if (bottle != null)
            {
                return true;
            }
            else
            {
                Debug.Log("Выкидывать можно только бутылки");
            }

            return false;
        }

        protected override void AddItem(ItemPlace place, IItem itemToAdd)
        {
            var bottle = itemToAdd as Bottle;
            
            if(bottle!=null)
                Destroy(bottle.gameObject);
        }
    }
}