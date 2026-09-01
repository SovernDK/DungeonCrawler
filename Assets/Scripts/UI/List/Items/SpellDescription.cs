using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpellDescription : MonoBehaviour
{
    public TMP_Text mana;
    public TMP_Text actions;
    public TMP_Text desc;

    public void SetSpellInfo(Spell spell)
    {
        mana.text = "Mana: " + spell.ManaCost;
        actions.text = "Action: " + spell.ActionCost;
        desc.text = spell.Description;
    }
}
