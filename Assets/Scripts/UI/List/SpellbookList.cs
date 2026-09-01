using System.Collections.Generic;
using AtlasGUI;
using UnityEngine;
using UnityEngine.UI;

public class SpellbookList : MonoBehaviour, IAtlasGuiList
{
    [SerializeField] 
    private GameObject itemPrefab;
    private MainContext mainContext;
    private int count;
    private List<GameObject> gameObjects;
    private Transform content;

    public SpellbookWindow window;

    public int Count { get => count; set => count = value; }

    public void Initialize(CoreContext context, Transform content)
    {
        this.content = content;
        mainContext = context as MainContext;

        gameObjects = new List<GameObject>();
        mainContext.player.Actor.Spellbook.Spells.ForEach(spell => 
        {
            GameObject goClone = Instantiate(itemPrefab, content);
            goClone.transform.GetChild(0).GetComponent<Image>().sprite = spell.Icon;
            goClone.GetComponent<ButtonInt>().Initialize(spell.Name, spell.Id);
            goClone.GetComponent<ButtonInt>().Connect(mainContext.player.CastSpell);
            goClone.GetComponent<SpellItem>().spellDescription = window.spellDescription;
            gameObjects.Add(goClone);
        });
    }

    public void Draw()
    {
        
    }

    public void Disable()
    {
        gameObjects.ForEach(item => item.GetComponent<Button>().enabled = false);
    }
}
