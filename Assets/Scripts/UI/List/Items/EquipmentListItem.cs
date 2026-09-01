using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class EquipmentListItem : MonoBehaviour, IAtlasListItem
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text slotLabel;
    [SerializeField] private TMP_Text itemLabel;
    [SerializeField] private Button unequip;

    private EquipmentSlot slot;

    public EquipmentSlot Slot { get => slot; set => slot = value; }
    public Button Use { get => unequip; set => unequip = value; }
    public Image Icon { get => icon; set => icon = value; }

    public void Initialize()
    {
        if(slot.Item != null)
            icon.sprite = slot.Item.Icon;
        slotLabel.text = slot.Name;
        itemLabel.text = slot.GetItem().Name;
    }

    public void Draw()
    {
        if(slot.Item != null)
            icon.sprite = slot.Item.Icon;
        slotLabel.text = slot.Name;
        itemLabel.text = slot.GetItem().Name;
    }
}
