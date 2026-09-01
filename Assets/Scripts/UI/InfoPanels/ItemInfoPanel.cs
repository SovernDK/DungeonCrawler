using Crawler.Actors;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoPanel : MonoBehaviour, IItemInfoPanel
{
    [TabGroup("General")]
    public TMP_Text title;

    [TabGroup("General")]
    public TMP_Text charges;
    
    [TabGroup("General")]
    public TMP_Text damage;

    [TabGroup("General")]
    public TMP_Text defence;

    [TabGroup("General")]
    public TMP_Text type;

    [TabGroup("General")]
    public TMP_Text exhaust;

    [TabGroup("General")]
    public TMP_Text description;

    [TabGroup("Resistances")]
    public GameObject resistanceItem;

    [TabGroup("Resistances")]
    public GameObject resistanceContent;

    [TabGroup("Resistances")]
    public GameObject resistances;

    [TabGroup("Resistances")]
    public Color negative;

    [TabGroup("Resistances")]
    public Color neutral;

    [TabGroup("Resistances")]
    public Color positive;

    [Button("Show")]
    public void Show()
    {
        Color col = GetComponent<Image>().color;
        col.a = 255;
        GetComponent<Image>().color = col;

        GetComponentsInChildren<TMP_Text>().ForEach(text => {
            Color col = text.color;
            col.a = 255;
            
            text.color = col;
        });
    }

    [Button("Hide")]
    public void Hide()
    { 
        Color col = GetComponent<Image>().color;
        col.a = 0;
        GetComponent<Image>().color = col;

        GetComponentsInChildren<TMP_Text>().ForEach(text => {
            Color col = text.color;
            col.a = 0;
            
            text.color = col;
        });
    }

    public void Draw(Item context)
    {
        title.text = context.Name.ToUpper();
        if(context.Consumable) 
        { 
            charges.transform.parent.gameObject.SetActive(true);
            charges.text = "Uses: " + context.Charges + "/" + context.MaxCharges;
        }
        else
        {
            charges.transform.parent.gameObject.SetActive(false);
        }
 
        if(context.Type == Item.EItemType.Usable && context.CanBeExhausted) 
        { 
            exhaust.transform.parent.gameObject.SetActive(true);
            exhaust.text = "Remains: " + context.CurrentTurnsToExhausted;
        }
        else
        {
            exhaust.transform.parent.gameObject.SetActive(false);
        }

        //DMG
        if(context.Type == Item.EItemType.Weapon) 
        { 
            damage.transform.parent.gameObject.SetActive(true);
            // Actor playerActor = (Gamemaster.Instance.context as MainContext).player.Actor;
            // Vector2 dmg = context.GetDamageValues(playerActor);
            // damage.text = "Damage: " + Mathf.Round(dmg.x) + " - " + Mathf.Round(dmg.y);
            damage.text = "Damage: " + context.Benefits.might;
        }
        else
        {
            damage.transform.parent.gameObject.SetActive(false);
        }

        if(context.Type == Item.EItemType.Armor) 
        {
            defence.transform.parent.gameObject.SetActive(true);
            defence.text = "Defence: " + context.Defence;
        }
        else
        {
            defence.transform.parent.gameObject.SetActive(false);
        }
            // resistances.gameObject.SetActive(true);
            // foreach(Transform item in resistanceContent.transform)
            // {
            //     item.gameObject.SetActive(false);
            // }

            // int i = 0;
            // context.Resistances.ForEach(resistance => 
            // {
            //     GameObject item = resistanceContent.transform.GetChild(i).gameObject;
            //     item.SetActive(true);
            //     if(resistance.Value > 0)
            //         item.GetComponentInChildren<TMP_Text>().color = positive;
            //     else if(resistance.Value < 0)
            //         item.GetComponentInChildren<TMP_Text>().color = negative;
            //     else 
            //         item.GetComponentInChildren<TMP_Text>().color = neutral;

            //     item.GetComponentInChildren<TMP_Text>().text = resistance.Element.ToString() + ": " + resistance.Value;
            //     i++;
            // });
        // }
        // else 
        // { 
        //     resistances.gameObject.SetActive(false);
        //     foreach(Transform item in resistanceContent.transform)
        //     {
        //         item.gameObject.SetActive(false);
        //     }
        // }

        switch(context.Type)
        {
            case Item.EItemType.Weapon:  
                type.transform.parent.gameObject.SetActive(true);
                type.text = "Type: " + context.WeaponType;
                break;
            case Item.EItemType.Armor:  
                type.transform.parent.gameObject.SetActive(true);
                type.text = "Type: " + context.ArmorType;
                break;
            case Item.EItemType.Consumable: 
                type.transform.parent.gameObject.SetActive(true);
                type.text = "Type: Consumable";
                break;
            default:
                type.transform.parent.gameObject.SetActive(false);
                break;
        }
        
        description.text = context.Description;
    }
}
