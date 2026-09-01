using System.Collections.Generic;
using Crawler;
using Crawler.FSM;
using Sirenix.OdinInspector;
using UnityEngine;

public class Lootable : Interactable
{
    private LootableData lootableData;
    [SerializeField][TabGroup("Lootable")] private ItemCard[] itemCards;

    private MainContext mainContext;

    public ItemCard[] ItemCards { get => itemCards; set => itemCards = value; }
    public LootableData LootableData { get => lootableData; set => lootableData = value; }

    public override void OnInit()
    {
        base.OnInit();
        mainContext = Gamemaster.Instance.context as MainContext;
    }

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
        player.Fsm.SetState((int) EPlayerState.Loot);
    }

    public virtual void Opened()
    {
        GetComponent<Collider>().enabled = false;
        mainContext = Gamemaster.Instance.context as MainContext;
        mainContext.player.Actor.InventoryController.Inventory.ClearOtherSlots();
 
        foreach(int id in lootableData.items)
        {
            if(id != -1) 
            {
                Item item = mainContext.database.items.Find(card => card.Item.Id == id).Item;
                mainContext.player.Actor.InventoryController.Inventory.AddItemToOther(item);
            }
        }

        mainContext.mainScene.OpenWindow("OtherInventory");
    }

    public virtual void Closed()
    {
        // Check what items remains in 'other slots' and save them to Feature.items as current state of this chest
        GetComponent<Collider>().enabled = true;
        mainContext.mainScene.CloseWindow("OtherInventory");
        List<int> restItems = new List<int>();
        foreach(InventorySlot slot in mainContext.player.Actor.InventoryController.Inventory.OtherSlots())
        {
            if(!slot.Item.Name.Equals("Empty"))
            {
                restItems.Add(slot.Item.Id);
            }
        }

        lootableData.items = restItems.ToArray();
        mainContext.mainScene.CloseWindow("OtherInventory");
    }
}
