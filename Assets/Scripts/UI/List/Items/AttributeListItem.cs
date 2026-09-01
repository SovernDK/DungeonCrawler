using AtlasGUI;
using Sheet;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttributeListItem : MonoBehaviour, IAtlasListItem
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private TMP_Text value;
    [SerializeField] private GameObject buttonsWrapper;

    [SerializeField] private Button increase, decrease;
    private Attribute attribute;
    
    public TMP_Text Label { get => label; set => label = value; }
    public TMP_Text Value { get => value; set => this.value = value; }
    public Attribute Attribute { get => attribute; set => attribute = value; }
    public Button Increase { get => increase; set => increase = value; }
    public Button Decrease { get => decrease; set => decrease = value; }

    public void Initialize()
    {
        label.text = attribute.Name;
        value.text = attribute.Value.ToString();
        
        // buttonsWrapper.SetActive(false);
    }

    public void Draw()
    {
        label.text = attribute.Name;
        value.text = attribute.Value.ToString();
    }

    public void ShowButtons()
    {
        // buttonsWrapper.SetActive(true);
    }

    public void HideButtons()
    {
        // buttonsWrapper.SetActive(false);
    }
}
