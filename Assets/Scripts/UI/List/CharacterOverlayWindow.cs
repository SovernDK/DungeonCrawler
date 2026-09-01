using System.Collections.Generic;
using UnityEngine;
using AtlasGUI;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class CharacterOverlayWindow : Window
{
    public TMP_Text level;
    public GameObject stateIconItem;
    public GameObject stateIcons;
    public GameObject arrows;
    public List<DerivedAttributeSlider> sliders;

    [BoxGroup("Action points")]
    public Transform content;

    [BoxGroup("Action points")]
    public GameObject actionPointItem;

    [BoxGroup("Action points")]
    public List<ActionPointItem> actionPoints;

    private List<GameObject> children = new List<GameObject>();

    public override void Initialize(CoreContext context)
    {
        base.Initialize(context);

        sliders.ForEach(slider => 
        {
            slider.Initialize((context as MainContext).player.Actor.CharacterSheet);
            (context as MainContext).player.Actor.CharacterSheet.onAttributeValueChange += slider.Draw;
            slider.Slider.onValueChanged.AddListener(slider.Shake);
        });

        (context as MainContext).player.Actor.CharacterSheet.onLevelUp += Draw;
        (context as MainContext).player.Actor.CharacterSheet.onCharacterStateAdded += Draw;
        (context as MainContext).player.Actor.InventoryController.Inventory.onArrowsChanged += Draw;
        (context as MainContext).player.Actor.OnEquipmentChangedEnd.AddListener(Draw);
        (context as MainContext).player.Actor.OnActionPointsChange.AddListener(Draw);

        for(int i = 0; i < 9; i++)
        {
            GameObject go = Instantiate(actionPointItem, content);
            actionPoints.Add(go.GetComponent<ActionPointItem>());
        }
        arrows.GetComponentInChildren<TMP_Text>().text = (context as MainContext).player.Actor.InventoryController.Inventory.Arrows.ToString();

        Draw();
    }

    public override void Draw()
    {
        base.Draw();

        sliders.ForEach(slider => 
        {
            slider.Initialize((context as MainContext).player.Actor.CharacterSheet);
        });

        // level.text = "Lv. " + (context as MainContext).player.Actor.CharacterSheet.Level;

        children.ForEach(child => {
            child.SetActive(false);
        });

        int i = 0;
        (context as MainContext).player.Actor.CharacterSheet.States.ForEach(state => 
        {
            if(i < children.Count)
            {
                children[i].SetActive(true);
                children[i].GetComponentInChildren<Image>().sprite = state.Icon;
            }
            else
            {
                GameObject go = Instantiate(stateIconItem, stateIcons.transform);
                go.GetComponentInChildren<Image>().sprite = state.Icon;
                children.Add(go);
            }
            i++;
        });

        actionPoints.ForEach(ap => {
            ap.gameObject.SetActive(false);
            ap.Active();
        });

        actionPoints.GetRange(0, (int) (context as MainContext).player.Actor.MaxActionPoints).ForEach(ap => {
            ap.gameObject.SetActive(true);
            ap.Unactive();
        });

        actionPoints.GetRange(0, (int) (context as MainContext).player.Actor.ActionPoints).ForEach(ap => {
            ap.Active();
        });

        arrows.GetComponentInChildren<TMP_Text>().text = (context as MainContext).player.Actor.InventoryController.Inventory.Arrows.ToString();
    }
}
