using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AtlasGUI;
using UnityEngine;

public class InventoryList : MonoBehaviour, IAtlasGuiList
{
    [SerializeField]
    private GameObject itemPrefab;
    private MainContext mainContext;
    private int count;
    private Item[] items;
    private List<GameObject> gameObjects;
    private Transform content;

    public int Count { get => count; set => count = value; }

    public void Initialize(CoreContext context, Transform content)
    {
        // this.content = content;
        // mainContext = context as MainContext;

        // count = mainContext.player.InventoryController.PlayerInventory.Items.Count;
        // items = mainContext.player.InventoryController.PlayerInventory.Items.ToArray();
        // gameObjects = new List<GameObject>();

        // for (int i = 0; i < count; i++)
        // {
        //     int itemId = items[i].Id;
        //     int id = i;
        //     Item item = items[i];

        //     GameObject goClone = Instantiate(itemPrefab, content);
        //     goClone.GetComponent<InventoryListItem>().Item = item;
        //     goClone.GetComponent<InventoryListItem>().Init();
            
        //     goClone.GetComponent<InventoryListItem>().Drop.onClick.AddListener(() => mainContext.player.InventoryController.PlayerInventory.RemoveItem(id));
        //     // goClone.GetComponent<InventoryListItem>().onHover.AddListener();
        //     if(item.Equippable)
        //     {
        //         goClone.GetComponent<InventoryListItem>().Use.onClick.AddListener(() => {
        //             Debug.Log(item.ToString());
        //             mainContext.player.InventoryController.PlayerEquipment.Equip(item.Slot, item);
        //             mainContext.player.InventoryController.PlayerInventory.RemoveItem(id);
        //         });
        //     }
        //     gameObjects.Add(goClone);
        // }

        // // Subscribe to item events
        // mainContext.player.InventoryController.PlayerInventory.onItemRemoved += Draw;
        // mainContext.player.InventoryController.PlayerInventory.onItemAdded += Draw;
    }

    public void Draw()
    {
        // count = mainContext.player.InventoryController.PlayerInventory.Items.Count;
        // items = mainContext.player.InventoryController.PlayerInventory.Items.ToArray();

        // foreach (GameObject go in gameObjects)
        // {
        //     go.SetActive(false);
        // }

        // for(int i = 0; i < count; i++)
        // {
        //     int itemId = items[i].Id;
        //     Item item = items[i];
        //     int id = i;
            
        //     if(i > gameObjects.Count - 1) 
        //     {
        //         GameObject goClone = Instantiate(itemPrefab, content);
        //         goClone.GetComponent<InventoryListItem>().Item = items[i];
        //         goClone.GetComponent<InventoryListItem>().Init();
        //         goClone.GetComponent<InventoryListItem>().Drop.onClick.AddListener(() => mainContext.player.InventoryController.PlayerInventory.RemoveItem(id));
        //         goClone.GetComponent<InventoryListItem>().Use.onClick.AddListener(() => {
        //             mainContext.player.InventoryController.PlayerEquipment.Equip(item.Slot, item);
        //             mainContext.player.InventoryController.PlayerInventory.RemoveItem(id);
        //         });
        //         gameObjects.Add(goClone);
        //     }
        //     else
        //     {
        //         GameObject goClone = gameObjects.ElementAt(i);
        //         goClone.SetActive(true);
        //         goClone.GetComponent<InventoryListItem>().Item = items[i];
        //         goClone.GetComponent<InventoryListItem>().Drop.onClick.RemoveAllListeners();
        //         goClone.GetComponent<InventoryListItem>().Drop.onClick.AddListener(() => mainContext.player.InventoryController.PlayerInventory.RemoveItem(id));
        //         goClone.GetComponent<InventoryListItem>().Use.onClick.AddListener(() => {
        //             Debug.Log(id);
        //             mainContext.player.InventoryController.PlayerEquipment.Equip(item.Slot, item);
        //             mainContext.player.InventoryController.PlayerInventory.RemoveItem(id);
        //         });
        //         goClone.GetComponent<InventoryListItem>().Draw();
        //     }
        // }
    }
}
