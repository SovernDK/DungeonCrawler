using UnityEngine;
using System.Collections.Generic;
using AtlasGUI;
using Sheet;
using System.Linq;

public class EquipmentList : MonoBehaviour, IAtlasGuiList
{
    [SerializeField]
    private GameObject itemPrefab;
    private MainContext mainContext;
    private int count;
    private EquipmentSlot[] items;
    private List<GameObject> gameObjects;
    private Transform content;
    
    public void Initialize(CoreContext context, Transform content)
    {
        this.content = content;
        mainContext = context as MainContext;

        count = mainContext.player.Actor.InventoryController.Equipment.Slots.Length;
        items = mainContext.player.Actor.InventoryController.Equipment.Slots;
        gameObjects = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            GameObject goClone = Instantiate(itemPrefab, content);
            goClone.GetComponent<EquipmentListItem>().Slot = items[i];
            goClone.GetComponent<EquipmentListItem>().Initialize();
            gameObjects.Add(goClone);
        }

        mainContext.player.Actor.InventoryController.Equipment.onEquiped += Draw;
    }

    public void Draw()
    {
        count = mainContext.player.Actor.InventoryController.Equipment.Slots.Length;
        items = mainContext.player.Actor.InventoryController.Equipment.Slots;

        for (int i = 0; i < count; i++)
        {
            GameObject goClone = gameObjects.ElementAt(i);
            goClone.GetComponent<EquipmentListItem>().Slot = items[i];
            goClone.GetComponent<EquipmentListItem>().Draw();
        }
    }
}
