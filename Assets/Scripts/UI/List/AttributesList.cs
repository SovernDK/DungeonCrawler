using UnityEngine;
using System.Collections.Generic;
using AtlasGUI;
using Sheet;

public class AttributesList : MonoBehaviour, IAtlasGuiList
{
    [SerializeField]
    private GameObject itemPrefab;
    private MainContext mainContext;
    private int count;
    private Attribute[] items;
    private List<GameObject> gameObjects;
    private Transform content;
    
    public void Initialize(CoreContext context, Transform content)
    {
        this.content = content;
        mainContext = context as MainContext;

        count = mainContext.player.Actor.CharacterSheet.Attributes.Length;
        items = mainContext.player.Actor.CharacterSheet.Attributes;
        gameObjects = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            GameObject goClone = Instantiate(itemPrefab, content);
            goClone.GetComponent<AttributeListItem>().Attribute = items[i];
            goClone.GetComponent<AttributeListItem>().Initialize();

            EAttribute attributeType = (EAttribute) i;
            // goClone.GetComponent<AttributeListItem>().Increase.onClick.AddListener(() => 
            // { 
            //     mainContext.player.Actor.CharacterSheet.LearnAttribute(attributeType, 1); 
            // });
            gameObjects.Add(goClone);
        }
    }

    public void Draw()
    {
        items = mainContext.player.Actor.CharacterSheet.Attributes;

        gameObjects.ForEach(item => 
        {
            item.GetComponent<AttributeListItem>().Draw();
        });

        if(mainContext.player.Actor.CharacterSheet.LearningPoints > 0)
        {
            gameObjects.ForEach(item =>
            {
                item.GetComponent<AttributeListItem>().ShowButtons();
            });
        }
        else
        {
            gameObjects.ForEach(item =>
            {
                item.GetComponent<AttributeListItem>().HideButtons();
            });
        }
    }
}
