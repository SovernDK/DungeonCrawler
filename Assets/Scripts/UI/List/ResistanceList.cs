using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

public class ResistanceList : MonoBehaviour, IAtlasGuiList
{ 
    [SerializeField]
    private GameObject itemPrefab;
    private MainContext mainContext;
    private Transform content;
    private List<GameObject> gameObjects;

    public void Initialize(CoreContext context, Transform content)
    {
        this.content = content;
        mainContext = context as MainContext;

        gameObjects = new List<GameObject>();
        mainContext.player.Actor.CharacterSheet.Resistances.ForEach(resistance => 
        {
            GameObject goClone = Instantiate(itemPrefab, content);
            ResistanceItem resistanceItem = goClone.GetComponent<ResistanceItem>();
            resistanceItem.Resistance = resistance;
            resistanceItem.Initialize();
            gameObjects.Add(goClone);
        });

        mainContext.player.Actor.CharacterSheet.onResistanceChanged += Draw;
    }

    public void Draw()
    {
        gameObjects.ForEach(gameObject => 
        {
            gameObject.GetComponent<ResistanceItem>().Draw();
        });
    }
}
