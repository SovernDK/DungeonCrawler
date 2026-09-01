using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemDraggable : Draggable
{
    public override void Initialize()
    {
        base.Initialize();
        
        draggableObject = transform.GetChild(0);
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if(TryGetComponent(out EquipmentItem equipment)) 
        {
            if(equipment.Item.Id != -1) 
            {
                Cursor.visible = false;
            }
            else
            {
                OnEndDrag(eventData);
                return;
            }
        }
        else if(TryGetComponent(out InventoryItem inventory)) 
        {
            if(inventory.Item.Id != -1) 
            {
                Cursor.visible = false;
            }
            else
            {
                OnEndDrag(eventData);
                return;
            }
        }
        
        base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {        
        if(TryGetComponent(out EquipmentItem equipment)) 
        {
            if(equipment.Item.Id == -1) 
            {
                OnEndDrag(eventData);
                return;
            }
        }
        else if(TryGetComponent(out InventoryItem inventory)) 
        {
            if(inventory.Item.Id == -1) 
            {
                OnEndDrag(eventData);
                return;
            }
        }
        
        base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        bool overUi = EventSystem.current.IsPointerOverGameObject();
        // eventData.pointerCurrentRaycast.gameObject.
        Cursor.visible = true;

        if(!overUi && TryGetComponent(out InventoryItem inventory))
        {
            (Gamemaster.Instance.context as MainContext).player.Actor.InventoryController.Inventory.RemoveItem(inventory.SlotId);
        }
    }
}
