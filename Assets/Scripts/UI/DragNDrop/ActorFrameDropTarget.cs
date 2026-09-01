using System.Collections;
using System.Collections.Generic;
using Crawler.Actors;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActorFrameDropTarget : DropTarget
{
    public override void OnDrop(PointerEventData eventData)
    {
        base.OnDrop(eventData);

        InventoryItem inventoryItem;
        if(draggable.TryGetComponent(out inventoryItem))
        {
            // USE ITEM
            if(inventoryItem.Item.Consumable)
            {
                Actor actor = (Gamemaster.Instance.context as MainContext).player.Actor;
                inventoryItem.Item.Use(actor, null);
            }
        }
    }
}
