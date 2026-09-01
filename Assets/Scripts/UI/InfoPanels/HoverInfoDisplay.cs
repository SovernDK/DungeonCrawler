using AtlasGUI;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverInfoDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private InventoryItem inventoryItem;
    private ItemInfoPanel infoPanel;

    public void Initialize()
    {
        infoPanel = FindObjectOfType<MainScene>().infoPanel;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Item item = GetItem();

        if(!item.Name.Equals("Empty")) 
        {
            infoPanel.gameObject.SetActive(true);
            // infoPanel.Show();
            infoPanel.Draw(item);

            UpdatePanelPosition();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // infoPanel.Hide();
        infoPanel.gameObject.SetActive(false);
    }

    private void UpdatePanelPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 0;

        infoPanel.transform.position = mousePosition + new Vector3(infoPanel.GetComponent<RectTransform>().rect.width / 2, -infoPanel.GetComponent<RectTransform>().rect.height / 2, 0);
    }

    private Item GetItem()
    {
        if(TryGetComponent(out InventoryItem inventoryItem))
        {
            return inventoryItem.Item;
        }
        else if(TryGetComponent(out EquipmentItem equipmentItem))
        {
            return equipmentItem.Item;
        }
        else
        {
            return new Item("Empty", -1);
        }
    }
}
