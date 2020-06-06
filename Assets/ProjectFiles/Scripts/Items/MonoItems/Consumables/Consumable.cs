using Player.Controllers;
using Player.PlayerStates;
using UnityEngine;
using UnityEngine.UIElements;

namespace Items.MonoItems.Consumables
{
    public abstract class Consumable : MonoItem,IConsumable
    {
        public Sprite Icon { get; protected set; }
        
        public float ConsumptionTime { get; protected set; }
        public int Volume { get=>volume; protected set=>volume = value; }
        [SerializeField] private int volume;
        
        public bool IsFull
        {
            get
            {
                if (Volume == 0)
                    return false;
                
                return CurrentVolume == Volume ? true : false;
            }
        }

        public bool IsEmpty => CurrentVolume == 0 ? true : false;
        
        public int CurrentVolume { get; protected set; }
        [SerializeField] private bool fillOnStart;

        public virtual void Fill(int count)
        {
            CurrentVolume = Mathf.Clamp(count, CurrentVolume, Volume);
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
            CurrentVolume = Mathf.Clamp(--CurrentVolume, 0, CurrentVolume);
        }

        public virtual void Clean()
        {
            CurrentVolume = 0;
        }

        protected override void Awake()
        {
            base.Awake();
            if(fillOnStart)
                FillAll();
        }
    }
}