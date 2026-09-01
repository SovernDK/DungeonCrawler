using UnityEngine;
using System.Collections.Generic;
using AtlasGUI;
using Sheet;

public class DerivedAttributesList : MonoBehaviour, IAtlasGuiList
{
       [SerializeField]
    private GameObject itemPrefab;
    private MainContext mainContext;
    private int count;
    private DerivedAttribute[] items;
    private List<GameObject> gameObjects;
    private Transform content;

    public void Initialize(CoreContext context, Transform content)
    {
        this.content = content;
        mainContext = context as MainContext;

        count = mainContext.player.Actor.CharacterSheet.DerivedAttributes.Length;
        items = mainContext.player.Actor.CharacterSheet.DerivedAttributes;
        gameObjects = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            GameObject goClone = Instantiate(itemPrefab, content);
            goClone.GetComponent<DerivedAttributesListItem>().Attribute = items[i];
            goClone.GetComponent<DerivedAttributesListItem>().Initialize();

            EDerivedAttribute attributeType = (EDerivedAttribute) i;
            // goClone.GetComponent<DerivedAttributesListItem>().Increase.onClick.AddListener(() => 
            // { 
            //     mainContext.player.Actor.CharacterSheet.LearnDerivedAttribute(attributeType, 10); mainContext.player.Actor.CharacterSheet.LearningPoints -= 1; 
            // });
            gameObjects.Add(goClone);
        }
    }

    public void Draw()
    {
        gameObjects.ForEach(item => 
        {
            item.GetComponent<DerivedAttributesListItem>().Draw();
        });

        if(mainContext.player.Actor.CharacterSheet.LearningPoints > 0)
        {
            gameObjects.ForEach(item =>
            {
                item.GetComponent<DerivedAttributesListItem>().ShowButtons();
            });
        }
        else
        {
            gameObjects.ForEach(item =>
            {
                item.GetComponent<DerivedAttributesListItem>().HideButtons();
            });
        }
    }
}
