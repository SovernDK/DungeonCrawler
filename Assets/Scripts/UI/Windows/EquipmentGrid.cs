using System.Collections.Generic;
using Sirenix.OdinInspector;
using AtlasGUI;
using UnityEngine;
using Crawler;

public class EquipmentGrid : Window
{
    public Transform content;
    public List<EquipmentItem> items;
    
    [InlineProperty]
    public EffectPrototype soundOnEquip;

    private MainContext mainContext;

    public MainContext MainContext { get => mainContext; set => mainContext = value; }

    public override void Initialize(CoreContext context)
    {
        base.Initialize(context);

        mainContext = context as MainContext;

        foreach(Transform child in content)
        {
            GameObject itemClone = child.gameObject;
            EquipmentItem item;
            if(itemClone.TryGetComponent(out item)) 
            {
                item.Item = new Item("Empty", -1);
                item.Initialize();
                items.Add(item);
            }
        }

        mainContext.player.Actor.InventoryController.Equipment.onEquiped += Draw;
        mainContext.player.Actor.InventoryController.Equipment.onUnequiped += Draw;
        
        Draw();
    }

    public override void Draw()
    {
        base.Draw();

        foreach(EquipmentItem slot in items)
        {
            slot.Item = mainContext.player.Actor.InventoryController.Equipment.Slots[(int)slot.Type].Item;
            slot.Draw();
        }
    }
}
