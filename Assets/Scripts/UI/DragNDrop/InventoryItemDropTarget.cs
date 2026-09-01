using Crawler;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemDropTarget : DropTarget
{
    [InlineProperty]
    public EffectPrototype effect;

    public override void OnDrop(PointerEventData eventData)
    {
        base.OnDrop(eventData);

        if(draggable == null) return;

        InventoryItem inventoryItem;
        if(draggable.TryGetComponent(out inventoryItem))
        {
            if(inventoryItem.Slot.Item.Id == -1) return;
            
            int slotA = inventoryItem.SlotId;
            int slotB = GetComponent<InventoryItem>().SlotId;
            
            bool result = (Gamemaster.Instance.context as MainContext).player.Actor.InventoryController.Inventory.SwapItems(slotA, slotB);
            if(result)
            {
                Gamemaster.Instance.effectsManager.AddEffect(effect.gameObject.name, transform.position, Quaternion.identity, effect.delay);
            }
            // Debug.Log("SlotA: "+slotA+", SlotB" + slotB+ ", result: "+result);
        }

        EquipmentItem equipmentItem;
        if(draggable.TryGetComponent(out equipmentItem))
        {
            Item item = equipmentItem.Item;
            EEquipmentSlot equipmentSlot = equipmentItem.Type;
            int slot = GetComponent<InventoryItem>().SlotId;

            bool result = (Gamemaster.Instance.context as MainContext).player.Actor.InventoryController.UnequipToSlot(equipmentSlot, slot);
            if(result)
            {
                Gamemaster.Instance.effectsManager.AddEffect(effect.gameObject.name, transform.position, Quaternion.identity, effect.delay);
            }
            // Debug.Log("SlotA: "+slotA+", SlotB" + slotB+ ", result: "+result);
        }
    }
}
