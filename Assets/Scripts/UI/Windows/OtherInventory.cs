using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AtlasGUI;
using Crawler;
using UnityEngine;

public class OtherInventory : Window
{
    public Transform content;
    public GameObject itemPrefab;
    public List<InventoryItem> items;

    private InventoryController inventoryController;

    private MainContext mainContext;

    public MainContext MainContext { get => mainContext; set => mainContext = value; }

    public override void Initialize(CoreContext context)
    {
        base.Initialize(context);

        mainContext = context as MainContext;

        // if(mainContext.player.CurrentInteractable != null) 
        // {
        //     mainContext.player.CurrentInteractable.TryGetComponent(out inventoryController);
        // }

        inventoryController = mainContext.player.Actor.InventoryController;

        if(inventoryController == null) return;

        InventorySlot[] slots = inventoryController.Inventory.InventorySlots;

        for(int i = 8; i < 16; i++)
        {
            GameObject itemClone = Instantiate(itemPrefab, content);
            InventoryItem item = itemClone.GetComponent<InventoryItem>();

            // item.Item = slots[i].Item;
            item.Slot = slots[i];
            item.SlotId = slots[i].SlotId;
            item.Initialize();

            items.Add(item);
        }

        inventoryController.Inventory.onItemAdded += Draw;
        inventoryController.Inventory.onItemRemoved += Draw;
        inventoryController.Inventory.onItemSwapped += Draw;

        Draw();
    }

    public override void Draw()
    {
        base.Draw();

        if(inventoryController == null) return;
        
        InventorySlot[] slots = inventoryController.Inventory.InventorySlots;

        for(int i = 8; i < 16; i++)
        {
            InventoryItem item = items.Find(item => item.GetComponent<InventoryItem>().SlotId == slots[i].SlotId);
            // item.Item = slots[i].Item;
            item.Draw();
        }
    }
}
