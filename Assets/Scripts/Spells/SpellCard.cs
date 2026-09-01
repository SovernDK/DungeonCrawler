using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName ="NewSpell", menuName = "Data/Spell")]
public class SpellCard : ScriptableObject
{
    [SerializeField] [InlineProperty] [HideLabel] private Spell spell;
    public Spell Spell { get => spell; set => spell = value; }
}
