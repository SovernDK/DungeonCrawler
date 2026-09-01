using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems; 
using UnityEngine.UI;

namespace AtlasGUI 
{
    public class InventoryListItem : MonoBehaviour, IAtlasListItem, IPointerEnterHandler
    {
        [SerializeField] private TMP_Text label;
        [SerializeField] private Image icon;
        [SerializeField] private Button use;
        [SerializeField] private Button drop;

        private Item item;

        public IntegerUnityEvent onUsed;
        public IntegerUnityEvent onDropped;
        public IntegerUnityEvent onHover;

        public Item Item { get => item; set => item = value; }
        public Button Use { get => use; set => use = value; }
        public Button Drop { get => drop; set => drop = value; }

        public void Initialize()
        {
            label.text = item.Name;
            icon.sprite = Item.Icon;
        }

        public void Draw()
        {
            label.text = item.Name;
            icon.sprite = Item.Icon;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            onHover.Invoke(Item.Id);
        }
    }

    [Serializable]
    public class IntegerUnityEvent : UnityEvent<int>  {}
}