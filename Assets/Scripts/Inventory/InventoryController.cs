using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Crawler 
{
    [Serializable]
    public class InventoryController
    {
        private MainContext context;

        [SerializeField] [TabGroup("Inventory")] 
        private int inventorySize;

        [SerializeField] [HideLabel] [InlineProperty] [TabGroup("Inventory")] 
        private Inventory inventory;

        [SerializeField] [HideLabel] [InlineProperty] [TabGroup("Equipment")] 
        private Equipment equipment;

        public Inventory Inventory { get => inventory; set => inventory = value; }
        public Equipment Equipment { get => equipment; set => equipment = value; }
        public int InventorySize { get => inventorySize; set => inventorySize = value; }

        public void Init(MainContext context)
        {
            this.context = context;

            inventory = new Inventory(inventorySize, context.config.emptyItem.Item);
            equipment = new Equipment(context.config.emptyItem.Item, context.database.slots);
        }

        public void EquipFromSlot(int inventorySlotId, EEquipmentSlot equipmentSlot)
        {
            Item item = inventory.InventorySlots[inventorySlotId].Item;
            Item equipmentSlotItem = equipment.Slots[(int) equipmentSlot].Item;
            if(item.Id != -1)
            {
                bool equipingResult = equipment.Equip(equipmentSlot, item);
                if(equipingResult) 
                {
                    inventory.RemoveItem(inventorySlotId);
                    if(equipmentSlotItem.Id != -1)
                    {
                        inventory.AddItem(inventorySlotId, equipmentSlotItem);
                    }

                    if(item.Type == Item.EItemType.Container)
                    {
                        inventory.ChangeContainerSize(item.InventorySlots);
                    }
                }
            }
        }

        public bool UnequipToSlot(EEquipmentSlot equipmentSlot, int inventorySlotId)
        {
            Item itemInSlot = inventory.InventorySlots[inventorySlotId].Item;
            Item equipmentSlotItem = equipment.Slots[(int) equipmentSlot].Item;

            if(equipmentSlot == EEquipmentSlot.Container && !inventory.ContainerSlotsEmpty())
            {
                Gamemaster.Instance.gamelog.NewEntry(null, null, "You cannot unequip container with items in it");
                return false;
            }
            if(itemInSlot.Id == -1)
            {
                equipment.Unequip(equipmentSlot);
                inventory.AddItem(inventorySlotId, equipmentSlotItem);
                if(equipmentSlotItem.Type == Item.EItemType.Container)
                {
                    inventory.ChangeContainerSize(0);
                }
                return true;
            }

            return false;
        }
    }
}