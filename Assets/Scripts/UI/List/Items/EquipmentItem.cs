using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentItem : MonoBehaviour, IAtlasListItem
{
    [SerializeField] private Image placeholderIcon;
    [SerializeField] private Image icon;
    [SerializeField] private EEquipmentSlot type;
    [SerializeField] private Item item;

    private Color placeholderIconColor;

    public Item Item { get => item; set => item = value; }
    public Image Icon { get => icon; set => icon = value; }
    public EEquipmentSlot Type { get => type; set => type = value; }
    
    public void Initialize()
    {
        GetComponent<InventoryItemDraggable>().Initialize();
        GetComponent<HoverInfoDisplay>().Initialize();

        placeholderIconColor = placeholderIcon.color;
        placeholderIcon.color = placeholderIconColor;
    }

    public void Draw()
    {
        if(!item.Name.Equals("Empty")) 
        {
            icon.sprite = item.Icon;
            icon.color = Color.white;

            placeholderIconColor.a = 0;
            placeholderIcon.color = placeholderIconColor;
        }
        else
        {
            placeholderIconColor.a = 0.5f;
            placeholderIcon.color = placeholderIconColor;

            Color col2 = Color.white;
            col2.a = 0;
            icon.color = col2;
        }
    }
}
