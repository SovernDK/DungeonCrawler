using System;
using System.Collections.Generic;
using Crawler.Actors;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Sheet 
{
    [Serializable]
    public class CharacterSheet
    {
        [SerializeField] [BoxGroup("Level")] private int level;
        [SerializeField] [BoxGroup("Level")] private int maxLevel;
        [SerializeField] [BoxGroup("Level")] private float experience;
        [SerializeField] [BoxGroup("Level")] private AnimationCurve experienceCurve;
        [SerializeField] [BoxGroup("Level")] private int learningPointsPerLevel;
        [SerializeField] [BoxGroup("Level")] private int learningPoints;
        [SerializeField] [BoxGroup("Level")] private int usedLearningPoints;
        [SerializeField] [ListDrawerSettings(ShowFoldout = false)] private List<CharacterState> states;
        [SerializeField] [ListDrawerSettings(ShowFoldout = false)] private Attribute[] attributes;
        [SerializeField] [ListDrawerSettings(ShowFoldout = false)] private DerivedAttribute[] derivedAttributes;
        [SerializeField] [ListDrawerSettings(ShowFoldout = false)] private Resistance[] resistances;
        [SerializeField] private Resistance[] baseResistances;
        private float[] experienceForLevel;

        public Attribute[] Attributes { get => attributes; set => attributes = value; }
        public DerivedAttribute[] DerivedAttributes { get => derivedAttributes; set => derivedAttributes = value; }
        public List<CharacterState> States { get => states; set => states = value; }
        public Resistance[] Resistances { get => resistances; set => resistances = value; }
        public int Level { get => level; set => level = value; }
        public Resistance[] BaseResistances { get => baseResistances; }
        public int LearningPoints { get => learningPoints; set => learningPoints = value; }

        #region Delegates
        public delegate void AttributeValueChangeDelegate();
        public event AttributeValueChangeDelegate onAttributeValueChange;

        public delegate void CharacterStateAdded();
        public event CharacterStateAdded onCharacterStateAdded;

        public delegate void ResistanceChanged();
        public event ResistanceChanged onResistanceChanged;

        public delegate void LevelUp();
        public event LevelUp onLevelUp;
        #endregion

        [Button("Load character sheet")]
        public void Init(float attributeMaxValue = 100, float derivedAttributeMaxValue = 100)
        {
            attributes = new Attribute[Enum.GetValues(typeof(EAttribute)).Length];
            derivedAttributes = new DerivedAttribute[Enum.GetValues(typeof(EDerivedAttribute)).Length];
            resistances = new Resistance[Enum.GetValues(typeof(EElement)).Length];
            baseResistances = new Resistance[Enum.GetValues(typeof(EElement)).Length];

            for(int i = 0; i < attributes.Length; i++)
            {
                string attrName = Enum.GetNames(typeof(EAttribute))[i];
                int index = (int) Enum.GetValues(typeof(EAttribute)).GetValue(i);
                attributes[index] = new Attribute(attrName, attributeMaxValue);
            }

            for(int i = 0; i < derivedAttributes.Length; i++)
            {
                string attrName = Enum.GetNames(typeof(EDerivedAttribute))[i];
                int index = (int) Enum.GetValues(typeof(EDerivedAttribute)).GetValue(i);
                derivedAttributes[index] = new DerivedAttribute(attrName, derivedAttributeMaxValue);
            }

            for(int i = 0; i < resistances.Length; i++)
            {
                string resName = Enum.GetNames(typeof(EElement))[i];
                int index = (int) Enum.GetValues(typeof(EElement)).GetValue(i);
                resistances[index] = new Resistance((EElement) i, resName, 0);
                baseResistances[index] = new Resistance((EElement) i, resName, 0);
            }

            experienceForLevel = new float[maxLevel];
            for(int i = 0; i < maxLevel; i++)
            {
                int lv = i + 1;
                experienceForLevel[i] = Mathf.Round(experienceCurve.Evaluate(NormalizeLevel(lv)) * 1000 * lv);
            }
        }

        private float NormalizeLevel(int level)
        {
            return (float) level / maxLevel;
        }

        public void ExecuteStatesAtBegginingOfTurn(Actor actor)
        {
            List<int> statesToBeRemoved = new List<int>();
            int i = 0;
            
            states.ForEach(state => {
                if(state.ExecuteWhen == EExecuteWhen.StartOfTurn) 
                {
                    state.Execute(actor);
                    if(state.CurrentTurnsToEnd == 0) statesToBeRemoved.Add(i);
                }
                i++;
            });

            if(statesToBeRemoved.Count > 0)
            {
                statesToBeRemoved.ForEach(index => states.RemoveAt(index));
                onCharacterStateAdded?.Invoke();
            }
        }

        public void ExecuteStatesAtEndOfTurn(Actor actor)
        {
            List<int> statesToBeRemoved = new List<int>();
            int i = 0;
            states.ForEach(state => {
                if(state.ExecuteWhen == EExecuteWhen.EndOfTurn)
                {
                    state.Execute(actor);
                    if(state.CurrentTurnsToEnd == 0) statesToBeRemoved.Add(state.Id);
                } 
                i++;
            });

            if(statesToBeRemoved.Count > 0)
            {
                statesToBeRemoved.ForEach(index => states.RemoveAt(index));
                onCharacterStateAdded?.Invoke();
            }
        }

        public void Change(EAttribute attribute, float value)
        {
            attributes[(int)attribute].Value = Mathf.Clamp(attributes[(int)attribute].Value + value, 0, attributes[(int)attribute].MaxValue);
            attributes[(int)attribute].Value = Mathf.Round(attributes[(int)attribute].Value);
            onAttributeValueChange?.Invoke();
        }

        public void Change(EDerivedAttribute attribute, float value)
        {
            derivedAttributes[(int)attribute].Value = Mathf.Clamp(derivedAttributes[(int)attribute].Value + value, 0, derivedAttributes[(int)attribute].MaxValue);
            derivedAttributes[(int)attribute].Value = Mathf.Round(derivedAttributes[(int)attribute].Value);
            onAttributeValueChange?.Invoke();
        }

        public void LearnAttribute(EAttribute attribute, float value)
        {    
            if(learningPoints > 0) 
            {
                attributes[(int)attribute].Value += value;
                learningPoints -= 1;
                onAttributeValueChange?.Invoke();
            }
        }

        public void LearnDerivedAttribute(EDerivedAttribute attribute, float value)
        {
            if(learningPoints > 0) 
            {
                derivedAttributes[(int)attribute].MaxValue += value;
                learningPoints -= 1;
                onAttributeValueChange?.Invoke();
            }
        }

        public void Change(EElement resistance, float value)
        {
            resistances[(int) resistance].Value += value;
            onResistanceChanged?.Invoke();
        }

        public void ChangeMax(EDerivedAttribute attribute, float value)
        {
            derivedAttributes[(int)attribute].MaxValue += value;
            onAttributeValueChange?.Invoke();
        }

        public void SetMax(EDerivedAttribute attribute, float value)
        {
            derivedAttributes[(int)attribute].MaxValue = value;
            if(derivedAttributes[(int)attribute].Value > derivedAttributes[(int)attribute].MaxValue)
            {
                derivedAttributes[(int)attribute].Value = value;
            }
            onAttributeValueChange?.Invoke();
        }

        public void Set(EDerivedAttribute attribute, float value)
        {
            derivedAttributes[(int)attribute].Value += value;
            onAttributeValueChange?.Invoke();
        }

        public void Set(EElement resistance, float value)
        {
            resistances[(int) resistance].Value = value;
            onResistanceChanged?.Invoke();
        }

        public void Set(EAttribute attribute, float value)
        {
            attributes[(int)attribute].Value = value;
            onAttributeValueChange?.Invoke();
        }

        public void ResetToBase(EDerivedAttribute attribute)
        {
            derivedAttributes[(int)attribute].MaxValue = derivedAttributes[(int)attribute].BaseValue;
            onAttributeValueChange?.Invoke();
        }

        public void ApplyState(CharacterState state)
        {
            if(states.Find(current => current.Id == state.Id) == null)
            { 
                states.Add(state.Copy());
                onCharacterStateAdded?.Invoke();
            }
        }

        public void RemoveState(CharacterState state)
        {
            int index = states.FindIndex(current => current.Id == state.Id);
            states.ForEach(current => Debug.Log("Id: " + current.Id) );
            if(index != -1)
            { 
                states.RemoveAt(index);
                onCharacterStateAdded?.Invoke();
            }
        }


        public void AddExp(float experience)
        {
            this.experience += experience;
            if(this.experience > experienceForLevel[level+1])
            {
                level++;
                learningPoints += learningPointsPerLevel;
                onLevelUp?.Invoke();
            }
        }

        public void AddLevel(int level)
        {
            this.level = Mathf.Clamp(this.level + level, 0, maxLevel);
            this.experience = experienceForLevel[this.level];
            learningPoints += learningPointsPerLevel;
            onLevelUp?.Invoke();
        }
    }

    [Serializable]
    public class Resistance
    {
        [SerializeField] private EElement element;
        [SerializeField] private float value;
        private string name;

        public string Name { get => name; set => name = value; }
        public float Value { get => value; set => this.value = value; }
        public EElement Element { get => element; set => element = value; }

        public Resistance(EElement element, string name, float value)
        {
            this.element = element;
            this.name = name;
            this.value = value;
        }
    }

    public enum EAttribute 
    {
        Might, Wisdom, Defence
    }

    public enum EDerivedAttribute 
    {
        Vitality, Mana, 
    }
}