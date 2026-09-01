using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Equipment
{
    private Item emptyItem;
    [SerializeField] private EquipmentSlot[] slots;
    public delegate void ItemEquipedDelegate();
    public delegate void ItemUnequipedDelegate();
    public event ItemEquipedDelegate onEquiped;
    public event ItemUnequipedDelegate onUnequiped;
    public EquipmentSlot[] Slots { get => slots; set => slots = value; }
    public Item EmptyItem { get => emptyItem; set => emptyItem = value; }

    public Equipment(Item emptyItem, List<EquipmentSlot> equipmentSlots)
    {
        this.emptyItem = emptyItem;

        slots = new EquipmentSlot[equipmentSlots.Count];
        equipmentSlots.ForEach(slot => 
        {
            slots[(int) slot.Type] = new EquipmentSlot(slot.Name, slot.Type)
            {
                AllowedArmorTypes = slot.AllowedArmorTypes,
                AllowedWeaponTypes = slot.AllowedWeaponTypes,
                AllowedItemTypes = slot.AllowedItemTypes
            };
            slots[(int) slot.Type].Equip(emptyItem);
        });
    }

    public bool Equip(EEquipmentSlot slot, Item item)
    {
        if(slots[(int) slot].IsAllowedToEquip(item))
        {
            slots[(int) slot].Equip(item);
            onEquiped?.Invoke();
            return true;
        }

        return false;
    }

    public void SwapSlots(EEquipmentSlot slotA, EEquipmentSlot slotB)
    {
        
    }

    public Item[] GetWeapons()
    {
        List<Item> weapons = new List<Item>();
        foreach(EquipmentSlot slot in slots)
        {
            if(slot.Item is Item weapon)
            {
                weapons.Add(weapon);
            }
        }

        return weapons.ToArray();
    }

    public bool Unequip(EEquipmentSlot slot)
    {
        slots[(int) slot].Equip(emptyItem);
        onEquiped?.Invoke();
        return true;
    }

    public Item[] GetArmor()
    {
        List<Item> armors = new List<Item>();
        slots.ForEach(slot => 
        {
            if(!slot.Item.Name.Equals("Empty") && slot.Item.Type == Item.EItemType.Armor) 
            {
                armors.Add(slot.Item);
            }
        });

        return armors.ToArray();
    }
}

[Serializable]
public class EquipmentSlot
{
    #region Inspector
    [SerializeField] [DisableInPlayMode] [PreviewField(Alignment = ObjectFieldAlignment.Left)] [HorizontalGroup("Item", Width = 50)] [HideLabel]
    private Sprite itemIcon;
    
    [SerializeField] [DisableInPlayMode] [VerticalGroup("Item/Values")]  [HideLabel]
    private string itemName;
    #endregion

    [SerializeField] [DisableInPlayMode] [VerticalGroup("Item/Values")] 
    private string name;
    
    [SerializeField] [DisableInPlayMode] [VerticalGroup("Item/Values")] 
    private EEquipmentSlot type;

    [SerializeField] [HideLabel] [DisableInPlayMode] [VerticalGroup("Item/Values")] 
    private List<Item.EWeaponType> allowedWeaponTypes;

    [SerializeField] [HideLabel] [DisableInPlayMode] [VerticalGroup("Item/Values")] 
    private List<Item.EArmorType> allowedArmorTypes;

    [SerializeField] [HideLabel] [DisableInPlayMode] [VerticalGroup("Item/Values")] 
    private List<Item.EItemType> allowedItemTypes;

    private Item item;

    public EEquipmentSlot Type { get => type; set => type = value; }
    public Item Item { get => item; set => item = value; }
    public string Name { get => name; set => name = value; }
    public List<Item.EWeaponType> AllowedWeaponTypes { get => allowedWeaponTypes; set => allowedWeaponTypes = value; }
    public List<Item.EArmorType> AllowedArmorTypes { get => allowedArmorTypes; set => allowedArmorTypes = value; }
    public List<Item.EItemType> AllowedItemTypes { get => allowedItemTypes; set => allowedItemTypes = value; }

    public EquipmentSlot(string name, EEquipmentSlot type = EEquipmentSlot.LeftHand)
    {
        this.name = name;
        this.type = type;
    }

    public void Equip(Item item)
    {
        this.item = item.Copy();
        this.itemName = item.Name;
        this.itemIcon = item.Icon;
    }

    public bool IsAllowedToEquip(Item item)
    {
        if(item.Type == Item.EItemType.Weapon)
        {
            return allowedWeaponTypes.Contains(item.WeaponType);
        }
        else if(item.Type == Item.EItemType.Armor)
        {
            return allowedArmorTypes.Contains(item.ArmorType);
        }
        else if(item.Type != Item.EItemType.Armor && item.Type != Item.EItemType.Weapon)
        {
            return allowedItemTypes.Contains(item.Type);
        }
        else
        {
            return false;
        }
    }

    public Item GetItem()
    {
        if(IsEquiped())
        {
            return item;
        }
        else
        {
            return new Item("Empty", -1);
        }
    }

    public bool IsEquiped()
    {
        return item != null;
    }
}

public enum EEquipmentSlot 
{
    LeftHand, RightHand, Head, Corpus, Shoes, Container
}