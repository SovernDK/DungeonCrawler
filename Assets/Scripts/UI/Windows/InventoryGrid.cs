using System.Collections.Generic;
using UnityEngine;
using AtlasGUI;
using Crawler;
using Sirenix.OdinInspector;

public class InventoryGrid : Window
{
    public Transform content;
    public GameObject itemPrefab;
    public List<InventoryItem> items;

    public int startIndex, endIndex;

    [InlineProperty]
    public EffectPrototype soundOnEquip;

    private InventoryController inventoryController;

    private MainContext mainContext;

    public MainContext MainContext { get => mainContext; set => mainContext = value; }

    public override void Initialize(CoreContext context)
    {
        base.Initialize(context);

        mainContext = context as MainContext;

        inventoryController = mainContext.player.Actor.InventoryController;
        if(inventoryController == null) return;

        InventorySlot[] slots = inventoryController.Inventory.InventorySlots;

        for(int i = startIndex; i < endIndex; i++)
        {
            GameObject itemClone = Instantiate(itemPrefab, content);
            InventoryItem item = itemClone.GetComponent<InventoryItem>();

            item.Slot = slots[i];
            item.SlotId = slots[i].SlotId;
            item.Initialize();

            items.Add(item);
        }

        inventoryController.Inventory.onItemAdded += Draw;
        inventoryController.Inventory.onItemRemoved += Draw;
        inventoryController.Inventory.onItemSwapped += Draw;
        inventoryController.Inventory.onContainerSizeChanged += Draw;

        Draw();
    }

    public override void Draw()
    {
        base.Draw();
        
        if(inventoryController == null) return;

        InventorySlot[] slots = inventoryController.Inventory.InventorySlots;
        
        for(int i = startIndex; i < endIndex; i++)
        {
            InventoryItem item = items.Find(item => item.GetComponent<InventoryItem>().SlotId == slots[i].SlotId);
            if(item.Slot.Disabled)
            {
                item.gameObject.SetActive(false);
            }
            else
            {
                item.gameObject.SetActive(true);   
                item.Draw();
            }  
        }
    }
}
