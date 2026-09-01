using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "Core/Config")]
public class Config : ScriptableObject
{
    public float attributeMaxValue;
    public float attributeMinValue;
    public float derivedAttributeMaxValue;
    public float derivedAttributeMinValue;

    [TabGroup("Special Items")]
    public ItemCard emptyItem;

    [TabGroup("Special Items")]
    public ItemCard lockpick;

    public GameObject buttonHoverSound;
    public GameObject buttonClickSound;
    public GameObject openWindowSound;
}
