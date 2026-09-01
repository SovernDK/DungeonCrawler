using System;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Crawler.Actors;

[Serializable]
public class Spell
{
    [SerializeField] [BoxGroup("General")] [HorizontalGroup("General/GeneralHoriz", Width = 50)] [HideLabel] [PreviewField(Alignment = ObjectFieldAlignment.Left)] 
    private Sprite icon;

    [SerializeField] [BoxGroup("General")] [VerticalGroup("General/GeneralHoriz/GeneralVerti")] [DisableInPlayMode] [ReadOnly]
    private int id;

    [SerializeField] [BoxGroup("General")] [VerticalGroup("General/GeneralHoriz/GeneralVerti")] [HideLabel]
    private string name;

    [SerializeField] [BoxGroup("General")] [VerticalGroup("General/GeneralHoriz/GeneralVerti")] [HideLabel] [Multiline]
    private string description;

    [SerializeField] [BoxGroup("Cost")] 
    private int manaCost;

    [SerializeField] [BoxGroup("Cost")] 
    private int actionCost;

    [SerializeField] [InlineEditor] 
    private List<Action> actions;

    [SerializeField]
    private int knockback;

    public string Name { get => name; set => name = value; }
    public List<Action> Actions { get => actions; set => actions = value; }
    public int Id { get => id; set => id = value; }
    public Sprite Icon { get => icon; set => icon = value; }
    public int ActionCost { get => actionCost; set => actionCost = value; }
    public int ManaCost { get => manaCost; set => manaCost = value; }
    public string Description { get => description; set => description = value; }

    public Spell(string name)
    {
        this.name = name;
    }

    public void Cast(Actor a, Actor b)
    {
        actions.ForEach(action => action.Execute(a, b));
    }

    public void PlayEffects(Actor target)
    {
        actions.ForEach(action => {
            action.PlayEffects(target);
        });
    }

    public Spell Copy()
    {
        Spell spell = new Spell(name)
        {
            name = name,
            icon = icon,
            actions = actions,
            manaCost = manaCost,
            actionCost = actionCost,
            description = description
        };
        return spell;
    }
}
