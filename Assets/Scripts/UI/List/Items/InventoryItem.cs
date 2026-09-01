using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IAtlasListItem, IPointerClickHandler
{
    [SerializeField] 
    private Image icon;

    [SerializeField] [InlineProperty] [HideLabel] 
    private InventorySlot slot;

    [SerializeField] 
    private int slotId;

    public int SlotId { get => slotId; set => slotId = value; }
    public Item Item { get => slot.Item; }
    public Image Icon { get => icon; set => icon = value; }
    public InventorySlot Slot { get => slot; set => slot = value; }

    public void Initialize()
    {
        GetComponent<InventoryItemDraggable>().Initialize();
        GetComponent<HoverInfoDisplay>().Initialize();
    }

    public void Draw()
    {
        if(slot.Item.Id != -1) 
        {
            icon.sprite = Item.Icon;
            icon.color = Color.white;
        }
        else
        {
            Color col = Color.white;
            col.a = 0;
            icon.color = col; 
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && (Item.Consumable || Item.Usable))
        {
            Item.Use((Gamemaster.Instance.context as MainContext).player.Actor, null);
            if(Item.Charges <= 0)
            {
                (Gamemaster.Instance.context as MainContext).player.Actor.InventoryController.Inventory.RemoveItem(SlotId);
            }
            Draw();
        }
    }
}
