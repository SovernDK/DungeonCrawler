using Crawler.Actors;
using Sheet;
using UnityEngine;
using System.Collections.Generic;
using Crawler;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "DamageAction", menuName = "Actions/DamageAction")]
public class DamageAction : Action
{
    [SerializeField] [HideLabel] [BoxGroup("General")] [HorizontalGroup("General/Types")] 
    private EActionDamageType damageType;

    [SerializeField] [HideLabel] [HorizontalGroup("General/Types")] [ShowIf("@damageType == EActionDamageType.DerivedAttribute")]
    private EDerivedAttribute targetDerivedAttribute;

    [SerializeField] [HideLabel] [HorizontalGroup("General/Types")] [ShowIf("@damageType == EActionDamageType.Attribute")]
    private EAttribute targetAttribute;

    [SerializeField] [HideLabel] [BoxGroup("Effects")]
    private List<EffectPrototype> effects;

    [SerializeField] [InlineProperty] [HideLabel] [BoxGroup("Formula")]
    private Formula formula;

    [SerializeField] [BoxGroup("Formula")] [Range(0, 100)]
    private float dmgVariance;

    [SerializeField] [BoxGroup("Formula")] [Range(0, 100)]
    private float critRate;

    [SerializeField] [BoxGroup("Formula")] [Range(0, 100)]
    private int range;

    [SerializeField] [BoxGroup("Formula")] [Range(0, 100)]
    private int area;

    public EActionDamageType DamageType { get => damageType; set => damageType = value; }
    public EAttribute TargetAttribute { get => targetAttribute; set => targetAttribute = value; }
    public EDerivedAttribute TargetDerivedAttribute { get => targetDerivedAttribute; set => targetDerivedAttribute = value; }
    public List<EffectPrototype> Effects { get => effects; set => effects = value; }
    public Formula Formula { get => formula; set => formula = value; }

    public override void Execute(Actor user, Actor other = null)
    {
        float value = formula.Parse(user, other);
        switch(Target)
        {
            case ETarget.Self:
                Do(user, value);
                break;
            case ETarget.Other:
                Do(other, value);
                break;
            default:
                Do(other, value);
                break;
        }
        
        base.Execute(user, other);
    }

    public void Do(Actor target, float value)
    {
        if(area > 0)
        {
            DoDamageArea(target, value);
        }
        else 
        {
            DoDamage(target, value);
        }
    }

    public void DoDamageArea(Actor target, float value)
    {
        List<Actor> actors = (Gamemaster.Instance.context as MainContext).grid.FindActorsInRange(target.CurrentTile.Tile.Coordinates, area);
        
        actors.ForEach(actor => 
        {
            Vector3 position = actor.transform.position + actor.DamageEffectAnchor;

            effects.ForEach(f => {
                Gamemaster.Instance.effectsManager.AddEffect(f.gameObject.name, position, Quaternion.LookRotation(Camera.main.transform.forward), f.delay, actor);
            });

            float currentValue = value;
            if(element != EElement.None) 
            {
                float resistance = 100 - actor.CharacterSheet.Resistances[(int) element].Value;
                currentValue = resistance / 100 * value; // Add resistances
            }

            currentValue *= (100 - Random.Range(0, dmgVariance)) / 100;

            //CRIT
            if(Random.Range(0, 101) < critRate)
            {
                currentValue *= Random.Range(2, 4);
            }

            switch(damageType)
            {
                case EActionDamageType.Attribute:
                    actor.CharacterSheet.Change(targetAttribute, -currentValue);
                    actor.Damaged();
                    break;
                case EActionDamageType.DerivedAttribute:
                    actor.CharacterSheet.Change(targetDerivedAttribute, -currentValue);
                    actor.Damaged();
                    break;
            }
        });
    }

    public void DoDamage(Actor target, float value)
    {
        float currentValue = value;
        if(element != EElement.None) 
        {
            float resistance = 100 - target.CharacterSheet.Resistances[(int) element].Value;
            currentValue = resistance / 100 * value; // Add resistances
        }

        currentValue *= (100 - Random.Range(0, dmgVariance+1)) / 100;

        //CRIT
        if(Random.Range(0, 101) < critRate)
        {
            currentValue *= Random.Range(2, 4);
        }

        switch(damageType)
        {
            case EActionDamageType.Attribute:
                target.CharacterSheet.Change(targetAttribute, -currentValue);
                target.Damaged();
                break;
            case EActionDamageType.DerivedAttribute:
                target.CharacterSheet.Change(targetDerivedAttribute, -currentValue);
                target.Damaged();
                break;
        }
    }


    public override void PlayEffects(Actor target)
    {
        base.PlayEffects(target);

        Vector3 position = target.transform.position + target.DamageEffectAnchor;

        effects.ForEach(f => {
            Gamemaster.Instance.effectsManager.AddEffect(f.gameObject.name, position, Quaternion.LookRotation(Camera.main.transform.forward), f.delay, target);
        });
    }

    public Vector2 ParseDamage(Actor a)
    {
        float valueMax = formula.Parse(a, Gamemaster.Instance.emptyActor);
        float valueMin = valueMax * (100 - dmgVariance) / 100;

        return new Vector2(valueMin, valueMax);
    }

    private bool HasEffectsScript(GameObject go)
    {
        return go != null && go.GetComponent<ParticleFx>() != null;
    }
}

public enum EActionDamageType
{
    Attribute, DerivedAttribute
}