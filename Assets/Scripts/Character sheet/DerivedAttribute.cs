using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Sheet 
{
    [Serializable]
    public class DerivedAttribute
    {
        [SerializeField] [PreviewField(Alignment = ObjectFieldAlignment.Left)] [HideLabel] [HorizontalGroup("Attribute", Width = 50)] 
        private Sprite icon;
        [SerializeField] [VerticalGroup("Attribute/Values")] 
        private string name;
        [SerializeField] [VerticalGroup("Attribute/Values")] 
        private float value;
        [SerializeField] [VerticalGroup("Attribute/Values")] 
        private float maxValue;
        [SerializeField] [VerticalGroup("Attribute/Values")]
        private float baseValue;

        public string Name { get => name; set => name = value; }
        public float Value { get => value; set => this.value = Mathf.Clamp(value, 0, maxValue); }
        public float MaxValue { get => maxValue; set => maxValue = value; }
        public Sprite Icon { get => icon; set => icon = value; }
        public float BaseValue { get => baseValue; set => baseValue = value; }

        public DerivedAttribute(string name, float maxValue)
        {
            this.name = name;
            this.maxValue = maxValue;
            this.value = maxValue;
            this.baseValue = maxValue;
        }
    }
}