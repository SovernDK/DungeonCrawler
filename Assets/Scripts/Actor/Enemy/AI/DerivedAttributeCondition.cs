using Crawler.Actors;
using Sheet;
using UnityEngine;
using Sirenix;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "NewDerivedAttributeCondition", menuName = "AI/DerivedAttributeCondition")]
public class DerivedAttributeCondition : Condition
{
    [HorizontalGroup("Condition")] [HideLabel] public EDerivedAttribute derivedAttribute;
    [HorizontalGroup("Condition")] [HideLabel] public EOperator operation;
    [HorizontalGroup("Condition")] [ShowIf("@valueType == EValueType.Static")] [HideLabel] public float value;
    [HorizontalGroup("Condition")] [ShowIf("@valueType == EValueType.Percent")] [HideLabel] [Range(0, 100)] public float percent;
    [HorizontalGroup("Condition")] [HideLabel] public EValueType valueType;

    public override bool Check(Actor actor)
    {
        if(valueType == EValueType.Static)
        {
            float currentValue = actor.CharacterSheet.DerivedAttributes[(int) derivedAttribute].Value;
            
            return CheckOperation(currentValue, value);
        }
        else {
            float currentValue = actor.CharacterSheet.DerivedAttributes[(int) derivedAttribute].Value;
            float maxValue = actor.CharacterSheet.DerivedAttributes[(int) derivedAttribute].MaxValue;
            float currentPercent = currentValue/maxValue * 100;
            
            return CheckOperation(currentPercent, percent);
        }
    }

    private bool CheckOperation(float leftSide, float rightSide)
    {
        switch(operation)
        {
            case EOperator.Equal:
                return leftSide == rightSide;
            case EOperator.GreaterThen:
                return leftSide > rightSide;
            case EOperator.LessThen:
                return leftSide < rightSide;
            case EOperator.GreaterOrEqual:
                return leftSide <= rightSide;
            case EOperator.LessOrEqual:
                return leftSide <= rightSide;
            default: 
                return leftSide == rightSide;
        }
    }
}

public enum EValueType
{
    Static, Percent
}

public enum EOperator 
{
    Equal, GreaterThen, LessThen, GreaterOrEqual, LessOrEqual
}