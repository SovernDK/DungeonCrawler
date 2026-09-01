using System.Collections;
using System.Collections.Generic;
using Crawler.Actors;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryAction", menuName = "Actions/InventoryAction")]
public class InventoryAction : Action
{
    [SerializeField] [BoxGroup("General")] 
    private EInventoryActionType inventoryActionType;

    [SerializeField] [ShowIf("@inventoryActionType == EInventoryActionType.AddArrows")] 
    private int arrows;

    public override void Execute(Actor user, Actor other)
    {
        base.Execute(user, other);
        if(inventoryActionType == EInventoryActionType.AddArrows)
        {
            user.InventoryController.Inventory.Arrows += arrows;
        }
    }
}

public enum EInventoryActionType
{
    AddContainerSlots, AddArrows
}