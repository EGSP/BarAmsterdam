using System;
using Core.ObjectPooling;
using TMPro;
using UnityEngine;

namespace World.BarElements
{
    public class OrderNotify: MonoBehaviour, IPoolObject
    {
        public TMP_Text Text
        {
            get
            {
                if (text != null)
                    return text;

                if (text == null)
                {
                    var tmpText = GetComponentInChildren<TMP_Text>();
                    text = tmpText;
                    if (tmpText != null)
                        return tmpText;
                }

                return null;
            }
        }
        [SerializeField] private TMP_Text text;
        
        public float Opacity { get; set; }
        
        public void Awake()
        {
            var tmpText = GetComponentInChildren<TMP_Text>();
            text = tmpText;
        }
        
        
        
        public IPoolContainer ParentPool { get; set; }
        public Action ReturnAction { get; set; }

        public void Dispose()
        {
            Destroy(gameObject);
        }

        public void ReturnToPool()
        { 
            gameObject.SetActive(false);
            ReturnAction();
        }

        public void AwakeFromPool()
        {
            gameObject.SetActive(true);
        }

        
    }
}