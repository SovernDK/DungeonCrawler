using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : Interactable
{
    public ItemCard itemCard;
    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnInteractedWith(Player player)
    {
        base.OnInteractedWith(player);
        
        player.Actor.InventoryController.Inventory.AddItem(itemCard.Item);
        PoolManager.Instance.ReturnToPool(gameObject);
    }
}
