using Crawler.Actors;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Sheet;

[Serializable]
public class Item
{
    [SerializeField] [BoxGroup("General")] [HorizontalGroup("General/GeneralHoriz", Width = 50)] [HideLabel] [PreviewField(Alignment = ObjectFieldAlignment.Left)] 
    private Sprite icon;
    
    [SerializeField] [VerticalGroup("General/GeneralHoriz/GeneralVerti")] [ReadOnly]
    private int id;

    [SerializeField] [VerticalGroup("General/GeneralHoriz/GeneralVerti")] 
    private string name;

    [SerializeField] [BoxGroup("General")] [Multiline]
    private string description;

    [SerializeField] [HorizontalGroup("States/StatesHoriz")] 
    private bool consumable;

    [SerializeField] [HorizontalGroup("States/StatesHoriz")] 
    private bool equippable;

    [SerializeField] [BoxGroup("Charges")] [HorizontalGroup("Charges/ChargesHoriz", VisibleIf = "@consumable == true")] 
    private float charges;

    [SerializeField] [BoxGroup("Charges")] [HorizontalGroup("Charges/ChargesHoriz", VisibleIf = "@consumable == true")] 
    private float maxCharges;

    [SerializeField] [HorizontalGroup("States/StatesHoriz")] 
    private bool canBeExhausted;

    [SerializeField] [BoxGroup("Exhausting", VisibleIf = "@canBeExhausted == true")]
    private int turnsToExhausted;

    [SerializeField] [BoxGroup("Exhausting")] [ReadOnly]
    private int currentTurnsToExhausted;

    [SerializeField] [InlineEditor] [HideLabel] [BoxGroup("Actions")] 
    private List<Action> actions;

    [SerializeField] [VerticalGroup("General/GeneralHoriz/GeneralVerti")] [HideLabel] 
    private EItemType type;

    [SerializeField] [BoxGroup("Weapon", VisibleIf = "@type == EItemType.Weapon")] 
    private int range;

    [SerializeField] [BoxGroup("Weapon")] 
    private EWeaponType weaponType;

    [SerializeField] [BoxGroup("States")] [HorizontalGroup("States/StatesHoriz")] 
    private bool isBinded;

    [SerializeField] [BoxGroup("Armor", VisibleIf = "@type == EItemType.Armor")]
    private EArmorType armorType;

    [SerializeField] [BoxGroup("Armor", VisibleIf = "@type == EItemType.Armor")]
    private int defence;

    [SerializeField] [BoxGroup("Armor")]
    private List<Resistance> resistances;

    [SerializeField] [BoxGroup("Consumable",  VisibleIf = "@type == EItemType.Consumable")] [PreviewField]
    private Sprite emptyIcon;

    [SerializeField] [HorizontalGroup("States/StatesHoriz")] 
    private bool usable;

    [SerializeField] [BoxGroup("Container")] [ShowIf("@type == EItemType.Container")]
    private int inventorySlots;

    [SerializeField] [BoxGroup("Benefits")] [InlineProperty] [HideLabel]
    private ItemBenefit benefits; 

    public Sprite Icon 
    { 
        get {
            if(consumable && charges == 0) return emptyIcon;
            else return icon;
        }
        set => icon = value; 
    }

    public string Name { get => name; set => name = value; }
    public int Id { get => id; set => id = value; }
    public bool Equipable { get => equippable; set => equippable = value; }
    public bool Consumable { get => consumable; set => consumable = value; }
    public float Charges { get => charges; set => charges = value; }
    public float MaxCharges { get => maxCharges; set => maxCharges = value; }
    public List<Action> Actions { get => actions; set => actions = value; }
    public EWeaponType WeaponType { get => weaponType; set => weaponType = value; }
    public bool IsBinded { get => isBinded; set => isBinded = value; }
    public EItemType Type { get => type; set => type = value; }
    public int Range { get => range; set => range = value; }
    public string Description { get => description; set => description = value; }
    public EArmorType ArmorType { get => armorType; set => armorType = value; }
    public List<Resistance> Resistances { get => resistances; set => resistances = value; }
    public bool Usable { get => usable; set => usable = value; }
    public int InventorySlots { get => inventorySlots; set => inventorySlots = value; }
    public bool CanBeExhausted { get => canBeExhausted; set => canBeExhausted = value; }
    public int TurnsToExhausted { get => turnsToExhausted; set => turnsToExhausted = value; }
    public int CurrentTurnsToExhausted { get => currentTurnsToExhausted; set => currentTurnsToExhausted = value; }
    public int Defence { get => defence; set => defence = value; }
    public ItemBenefit Benefits { get => benefits; set => benefits = value; }


    public Item(string name, int id)
    {
        this.name = name;
        this.id = id;

        charges = maxCharges;
    }

    public override string ToString()
    {
        string equippableText = equippable ? "Yes" : "No";
        return $"Name: {name}, ID: {id}, Equippable: {equippableText}";
    }

    public void Use(Actor user, Actor other)
    {
        if(actions.Count <= 0)
        {
            Debug.LogError("There are no actions assigned to item you trying to use!");
        }
        
        if(consumable && charges > 0) 
        {
            actions.ForEach(action => {
                action.Execute(user, other);
            });
            charges = Mathf.Clamp(charges-1, 0, maxCharges);
        }
        else if(!consumable)
        {
            actions.ForEach(action => {
                action.Execute(user, other);
            });
        }
    }

    public void PlayEffects(Actor target)
    {
        actions.ForEach(action => {
            action.PlayEffects(target);
        });
    }

    public Item Copy()
    {
        Item copy = new Item(name, id)
        {
            icon = icon,
            description = description,
            consumable = consumable,
            equippable = equippable,
            charges = charges,
            maxCharges = maxCharges,
            actions = new List<Action>(actions),
            type = type,
            range = range,
            weaponType = weaponType,
            isBinded = isBinded,
            armorType = armorType,
            resistances = new List<Resistance>(resistances),
            emptyIcon = emptyIcon,
            usable = usable,
            inventorySlots = inventorySlots,
            canBeExhausted = canBeExhausted,
            turnsToExhausted = turnsToExhausted,
            currentTurnsToExhausted = turnsToExhausted,
            defence = defence,
            benefits = benefits
        };
        return copy;
    }

    public Vector2 GetDamageValues(Actor a)
    {
        if(actions.Count > 0)
            return (actions[0] as DamageAction).ParseDamage(a);
        else
            return Vector2.one;
    }

    public enum EWeaponType 
    {
        Sword, Axe, Blunt, Dagger, Bow, Offhand
    }

    public enum EArmorType 
    {
        Helmet, Armor, Shoes, Shield
    }

    public enum EItemType 
    {
        Weapon, Armor, Consumable, Map, Usable, Container
    }
}

[Serializable]
public class ItemBenefit
{
    public float actionPoints;
    public float maxHP;
    public float maxMana;
    public float might;
    public float wisdom;
}