using UnityEngine;
using Sheet;
using TMPro;

public class ResistanceItem : MonoBehaviour, IAtlasListItem
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private TMP_Text value;
    private Resistance resistance;

    public Resistance Resistance { get => resistance; set => resistance = value; }

    public void Initialize()
    {
        label.text = resistance.Name;
        value.text = resistance.Value.ToString();
    }

    public void Draw()
    {
        label.text = resistance.Name;
        value.text = resistance.Value.ToString();
    }
}
