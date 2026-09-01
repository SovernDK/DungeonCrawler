using AtlasGUI;
using TMPro;
using UnityEngine;

public class StatsWindow : Window
{
    public Transform attributeContent;
    public Transform derivedAttributeContent;
    public Transform resistanceContent;
    private AttributesList attributeList;
    private DerivedAttributesList derivedAttributeList;
    private ResistanceList resistanceList;

    [SerializeField]
    private TMP_Text levelValue;

    [SerializeField]
    private TMP_Text lpValue;

    public override void Initialize(CoreContext context)
    {
        base.Initialize(context);

        attributeList = GetComponent<AttributesList>();
        attributeList.Initialize(context, attributeContent);

        derivedAttributeList = GetComponent<DerivedAttributesList>();
        derivedAttributeList.Initialize(context, derivedAttributeContent);

        resistanceList = GetComponent<ResistanceList>();
        resistanceList.Initialize(context, resistanceContent);

        // levelValue.text = (context as MainContext).player.Actor.CharacterSheet.Level.ToString();
        // lpValue.text = (context as MainContext).player.Actor.CharacterSheet.LearningPoints.ToString();

        (context as MainContext).player.Actor.CharacterSheet.onLevelUp += Draw;
        (context as MainContext).player.Actor.CharacterSheet.onAttributeValueChange += Draw;
    }

    public override void Draw()
    {
        base.Draw();

        // levelValue.text = (context as MainContext).player.Actor.CharacterSheet.Level.ToString();
        // lpValue.text = (context as MainContext).player.Actor.CharacterSheet.LearningPoints.ToString();
        
        attributeList.Draw();
        derivedAttributeList.Draw();
    }
}
