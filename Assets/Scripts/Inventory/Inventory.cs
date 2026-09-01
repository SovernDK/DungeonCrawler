using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class Inventory
{
    private int inventorySize;
    private int containerSize;
    private int arrows;
    private Item emptyItem;

    public delegate void ItemAddedDelegate();
    public delegate void ItemRemovedDelegate();
    public delegate void ItemSwappedDelegate();
    public delegate void ContainerSizeChanged();
    public delegate void ArrowsChanged();

    public event ItemAddedDelegate onItemAdded;
    public event ItemRemovedDelegate onItemRemoved;
    public event ItemSwappedDelegate onItemSwapped;
    public event ContainerSizeChanged onContainerSizeChanged;
    public event ArrowsChanged onArrowsChanged;

    [SerializeField] [ListDrawerSettings(NumberOfItemsPerPage = 6)]
    private InventorySlot[] inventorySlots;

    public InventorySlot[] InventorySlots { get => inventorySlots; set => inventorySlots = value; }
    public Item EmptyItem { get => emptyItem; set => emptyItem = value; }
    public int ContainerSize { get => containerSize; }
    public int Arrows { 
        get => arrows;
        set {
            arrows = value;
            onArrowsChanged?.Invoke();
        }
    }


    public Inventory(int inventorySize, Item empty)
    {
        this.inventorySize = inventorySize;
        this.emptyItem = empty;

        inventorySlots = new InventorySlot[inventorySize];
        for(int i = 0; i < inventorySize; i++)
        {
            inventorySlots[i] = new InventorySlot(i);
            inventorySlots[i].SetItem(emptyItem);
        }
    }

    public bool AddItemToOther(Item item)
    {
        for(int i = 8; i < 16; i++)
        {
            if(inventorySlots[i].Item.Id == -1) 
            {
                inventorySlots[i].SetItem(item);
                onItemAdded?.Invoke();
                return true;
            }
        }

        return false;
    }

    public bool AddItem(Item item)
    {
        foreach(InventorySlot slot in inventorySlots)
        {
            if(slot.Item.Id == -1 && slot.SlotId >= 0 && slot.SlotId < inventorySize) 
            {
                slot.SetItem(item);
                onItemAdded?.Invoke();
                return true;
            }
        }

        return false;
    }

    public bool AddItem(int slotId, Item item)
    {
        if(slotId >= 0 && slotId < inventorySize)
        {
            inventorySlots[slotId].SetItem(item);
            onItemAdded?.Invoke();
            return true;
        }

        return false;
    }

    public bool SwapItems(int slotA, int slotB)
    {
        if(slotA >= 0 && slotA < inventorySize && slotB >= 0 && slotB < inventorySize)
        {
            Item itemA = inventorySlots[slotA].Item;
            Item itemB = inventorySlots[slotB].Item;

            if(itemA.Equals(emptyItem))
            {
                return false;
            }

            inventorySlots[slotA].SetItem(itemB);
            inventorySlots[slotB].SetItem(itemA);

            onItemSwapped?.Invoke();
            return true;
        }

        return false;
    }

    public void ClearOtherSlots()
    {
        for(int i = 8; i < 16; i++)
        {
            inventorySlots[i].SetItem(emptyItem);
        }
    }

    public InventorySlot[] OtherSlots()
    {
        InventorySlot[] otherSlots = new InventorySlot[8];

        for(int i = 8; i < 16; i++) 
        {
            otherSlots[i-8] = inventorySlots[i];
        }
        return otherSlots;
    }

    public InventorySlot[] ContainerSlots()
    {
        List<InventorySlot> slots = new List<InventorySlot>();

        for(int i = 16; i < 30; i++) 
        {
            slots.Add(inventorySlots[i]);
        }
        return slots.ToArray();
    }

    public InventorySlot[] Slots()
    {
        InventorySlot[] otherSlots = new InventorySlot[8];

        for(int i = 0; i < 8; i++) 
        {
            otherSlots[i] = inventorySlots[i];
        }
        return otherSlots;
    }

    public void RemoveItem(int slotId)
    {
        if(slotId >= 0 && slotId < inventorySize) 
        {
            inventorySlots[slotId].SetItem(emptyItem);
            onItemRemoved?.Invoke();
        }
    }

    public bool ContainsItem(int itemId)
    {
        foreach (InventorySlot slot in Slots())
        {
            if (slot.Item.Id == itemId)
            {
                return true;
            }
        }
        return false;
    }

    public void ChangeContainerSize(int i)
    {
        InventorySlot[] containerSlots = ContainerSlots();
        containerSize = i;

        containerSlots.ToList().ForEach(slot =>
        {
            slot.Disabled = true;
        });

        containerSlots.ToList().GetRange(0, i).ForEach(slot =>
        {
            slot.Disabled = false;
        });

        onContainerSizeChanged.Invoke();
    }

    public bool ContainerSlotsEmpty()
    {
        bool result = true;
        InventorySlot[] containerSlots = ContainerSlots();
        containerSlots.ToList().ForEach(slot => {
            if(slot.Item.Id != -1 && !slot.Disabled)
                result = false;
        });

        return result;
    }
}

[Serializable]
public class InventorySlot 
{
    [SerializeField] [PreviewField(Alignment = ObjectFieldAlignment.Left)] [HorizontalGroup("Item", Width = 50)] [HideLabel]
    private Sprite itemIcon;
    
    [SerializeField] [ReadOnly] [VerticalGroup("Item/Values")] 
    private int slotId;

    [SerializeField] [VerticalGroup("Item/Values")]  [HideLabel]
    private string itemName;

    [SerializeField]
    private bool disabled = false;
    
    private Item item;

    public int SlotId { get => slotId; set => slotId = value; }
    public Item Item { get => item; set => item = value; }
    public bool Disabled { get => disabled; set => disabled = value; }

    public InventorySlot(int id)
    {
        slotId = id;
    }

    public void SetItem(Item item)
    {
        this.item = item.Copy();
        this.itemName = this.item.Name;
        this.itemIcon = this.item.Icon;
    }
}