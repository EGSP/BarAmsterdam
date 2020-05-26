using Player.Controllers;
using Player.PlayerStates;
using UnityEngine;

namespace Items.MonoItems.Consumables
{
    public abstract class Consumable : MonoItem,IConsumable
    {
        public Sprite Icon { get; protected set; }
        
        public float ConsumptionTime { get; protected set; }
        public int Volume { get=>volume; protected set=>volume = value; }
        [SerializeField] private int volume;
        
        public bool IsFull => CurrentVolume == Volume ? true : false;
        
        public bool IsEmpty => CurrentVolume == 0 ? true : false;
        
        public int CurrentVolume { get; protected set; }

        public virtual void Fill(int count)
        {
            CurrentVolume = Mathf.Clamp(CurrentVolume, CurrentVolume, count);
        }

        /// <summary>
        /// Наполняет весь объем
        /// </summary>
        public virtual void FillAll()
        {
            CurrentVolume = Volume;
        }

        public virtual void Consume()
        {
            CurrentVolume = Mathf.Clamp(CurrentVolume, 0, CurrentVolume);
        }

        public virtual void Clean()
        {
            CurrentVolume = 0;
        }
    }
}