using System;
using System.Collections;
using System.Collections.Generic;
using Crawler.Actors;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class AgentAction
{
    [SerializeField] [InlineEditor] [HideLabel] [ListDrawerSettings(DefaultExpandedState = true, ShowFoldout = false)] private List<Condition> conditions;
    [SerializeField] [HideLabel] [ValueDropdown("GetValues")] private string command;
    [SerializeField] [InlineEditor] [ShowIf("CommandIsSpellCast")] private SpellCard spell;
    [SerializeField] [BoxGroup("State")] [HorizontalGroup("State/Horiz")] private float priority;
    [SerializeField] [BoxGroup("State")] [HorizontalGroup("State/Horiz")] [ReadOnly] private float weight;

    public List<Condition> Conditions { get => conditions; set => conditions = value; }
    public string Command { get => command; set => command = value; }
    public float Weight { get => weight; set => weight = value; }
    public SpellCard Spell { get => spell; set => spell = value; }
    public float Priority { get => priority; set => priority = value; }

    public bool Check(Actor a)
    {
        bool conditionsFullfilled = true;
        conditions.ForEach(cond => { if(!cond.Check(a)) conditionsFullfilled = false; });
        return conditionsFullfilled;
    }

    public string[] GetCommands()
    {
        return new string[4];
    }

    private static IEnumerable GetValues()
    {
        return new ValueDropdownList<string>()
        {
            { "MoveForward", "MoveForward" },
            { "MoveBack", "MoveBack" },
            { "MoveLeft", "MoveLeft" },
            { "MoveRight", "MoveRight" },
            { "MoveToward", "MoveToward" },
            { "RotateRight", "RotateRight" },
            { "RotateLeft", "RotateLeft" },
            { "Attack", "Attack" },
            { "SpellCast", "SpellCast" },
        };
    } 

    private bool CommandIsSpellCast()
    {
        return command == "SpellCast";
    }
}
