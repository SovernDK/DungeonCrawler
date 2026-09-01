using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Sheet 
{
    [Serializable]
    public class Attribute
    {
        [SerializeField] [PreviewField(Alignment = ObjectFieldAlignment.Left)] [HideLabel] [HorizontalGroup("Attribute", Width = 50)] 
        private Sprite icon;
        [SerializeField] [VerticalGroup("Attribute/Values")] 
        private string name;
        [SerializeField] [VerticalGroup("Attribute/Values")]
        private float value;
        [SerializeField] [VerticalGroup("Attribute/Values")] 
        private float maxValue;

        public string Name { get => name; set => name = value; }
        public float Value { get => value; set => this.value = Mathf.Clamp(value, 0, maxValue); }
        public float MaxValue { get => maxValue; set => maxValue = value; }

        public Attribute(string name, float maxValue)
        {
            this.name = name;
            this.maxValue = maxValue;
            this.value = 1;
        }
    }
}
