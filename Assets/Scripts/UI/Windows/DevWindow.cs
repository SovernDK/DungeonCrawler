using AtlasGUI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class DevWindow : Window
{
    [BoxGroup("Items")]
    public GameObject itemsPrefab;

    [BoxGroup("Items")]
    public Transform itemsContent;

    [BoxGroup("Level")]
    public Button increase;

    [BoxGroup("Level")]
    public Button decrease;

    private MainContext mainContext;
    public MainContext MainContext { get => mainContext; set => mainContext = value; }

    public override void Initialize(CoreContext context)
    {
        base.Initialize(context);

        mainContext = context as MainContext;

        mainContext.database.items.ForEach(item =>
        {
            GameObject itemPrefab = Instantiate(itemsPrefab, itemsContent);
            itemPrefab.GetComponent<ButtonInt>().Initialize("", item.Item.Id);
            itemPrefab.GetComponent<ButtonInt>().Connect(_ => {
                mainContext.player.Actor.InventoryController.Inventory.AddItem(item.Item);
            });
            itemPrefab.GetComponent<DevItem>().Item = item.Item;
            itemPrefab.GetComponent<DevItem>().Initialize();
        });

        increase.onClick.AddListener(() => { mainContext.player.Actor.CharacterSheet.AddLevel(1); });
        decrease.onClick.AddListener(() => { mainContext.player.Actor.CharacterSheet.AddLevel(-1); });
    }

    public override void Draw()
    {
        base.Draw();
    }
}
