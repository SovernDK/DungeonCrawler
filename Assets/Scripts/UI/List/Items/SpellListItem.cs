using TMPro;
using UnityEngine;

public class SpellListItem : MonoBehaviour, IAtlasListItem
{
    [SerializeField] private TMP_Text label;
    private Spell spell;
    
    public TMP_Text Label { get => label; set => label = value; }
    public Spell Spell { get => spell; set => spell = value; }

    public void Initialize()
    {
        label.text = spell.Name;
    }

    public void Draw()
    {
        label.text = spell.Name;
    }
}
