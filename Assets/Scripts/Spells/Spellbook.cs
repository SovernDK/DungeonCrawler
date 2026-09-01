using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sheet 
{
    [Serializable]
    public class Spellbook
    {
        [SerializeField] private List<SpellCard> startingSpells;
        [SerializeField] private List<Spell> spells = new List<Spell>();

        public List<Spell> Spells { get => spells; set => spells = value; }

        public void Init()
        {
            foreach(SpellCard card in startingSpells)
            {
                spells.Add(card.Spell);
            }
        }
    }
}