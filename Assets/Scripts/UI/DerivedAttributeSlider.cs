using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sheet;
using UnityEngine;
using UnityEngine.UI;

public class DerivedAttributeSlider : MonoBehaviour, IAtlasListItem
{
    [SerializeField] private Slider slider;
    [SerializeField] private EDerivedAttribute type;
    private CharacterSheet sheet;
    private DerivedAttribute derivedAttribute;

    public Slider Slider { get => slider; set => slider = value; }
    public CharacterSheet Sheet { get => sheet; set => sheet = value; }
    public EDerivedAttribute Type { get => type; set => type = value; }

    public void Initialize(CharacterSheet sheet)
    {
        this.sheet = sheet;
        
        derivedAttribute = sheet.DerivedAttributes[(int) type];
        slider.maxValue = derivedAttribute.MaxValue;
        slider.minValue = 0;
        slider.value = derivedAttribute.Value;
    }
    
    public void Draw()
    {
        slider.value = derivedAttribute.Value;
    }

    public void Shake(float val)
    {
        slider.GetComponent<RectTransform>().DOShakeAnchorPos(.25f, 5, 10, 90, false, true, ShakeRandomnessMode.Full);
    }

    public void Initialize()
    {
        
    }
}
