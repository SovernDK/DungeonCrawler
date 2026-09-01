using UnityEngine;
using AtlasGUI;
using Sirenix.Utilities;
using System.Collections.Generic;
using Crawler;

public class InventoryWindow : Window
{
    public GameObject inventoryItem;
    public GameObject containerWindow;
    public List<InventoryItem> items;
    private InventoryController inventoryController;

    public override void Initialize(CoreContext context)
    {
        base.Initialize(context);

        (context as MainContext).player.Actor.InventoryController.Inventory.onContainerSizeChanged += Draw;
    }

    public override void Draw()
    {
        base.Draw();

        Inventory inventory = (context as MainContext).player.Actor.InventoryController.Inventory;
        int containerSize = inventory.ContainerSize;
        if(containerSize > 0)
        {
            containerWindow.SetActive(true);
        }
        else
        {
            containerWindow.SetActive(false);
        }
    }
}