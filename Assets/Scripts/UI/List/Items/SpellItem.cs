using AtlasGUI;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellItem : MonoBehaviour, IPointerEnterHandler
{
    public SpellDescription spellDescription;
    public void OnPointerEnter(PointerEventData eventData)
    {
        int id = GetComponent<ButtonInt>().value;
        Spell spell = (Gamemaster.Instance.context as MainContext).database.spells.Find(card => card.Spell.Id == id).Spell;
        spellDescription.SetSpellInfo(spell);
    }
}
