using System;
using System.Collections;
using System.Collections.Generic;
using Crawler;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentItemDropTarget : DropTarget
{
    [InlineProperty]
    public EffectPrototype effect;

    public override void OnDrop(PointerEventData eventData)
    {
        base.OnDrop(eventData);

        if(draggable != null) 
        {
            if(draggable.TryGetComponent(out InventoryItem inventoryItem))
            {
                int slotItem = inventoryItem.SlotId;
                EEquipmentSlot slotEquipment = GetComponent<EquipmentItem>().Type;
                
                (Gamemaster.Instance.context as MainContext).player.Actor.InventoryController.EquipFromSlot(slotItem, slotEquipment);
                Gamemaster.Instance.effectsManager.AddEffect(effect.gameObject.name, transform.position, Quaternion.identity, effect.delay);
            }

            if(draggable.TryGetComponent(out EquipmentItem equipmentItem))
            {
                // EEquipmentSlot slotItem = equipmentItem.Type;
                // EEquipmentSlot slotEquipment = GetComponent<EquipmentItem>().Type;
                
                // (Gamemaster.Instance.context as MainContext).player.InventoryController.EquipFromSlot(slotItem, slotEquipment);
            }
        }
    }
}
