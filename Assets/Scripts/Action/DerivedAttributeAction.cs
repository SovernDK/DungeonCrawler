using Crawler.Actors;
using Sheet;
using UnityEngine;
using System.Collections.Generic;
using Crawler;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "DerivedAttributeAction", menuName = "Actions/DerivedAttributeAction")]
public class DerivedAttributeAction : Action
{
    [HideLabel] [HorizontalGroup("Types")]
    public EDerivedAttribute derivedAttribute;
    public List<EffectPrototype> effects;

    [InlineProperty] [HideLabel]
    public Formula formula;

    public override void Execute(Actor user, Actor other = null)
    {
        float value = formula.Parse(user, other);

        switch(Target)
        {
            case ETarget.Self:
                user.CharacterSheet.Change(derivedAttribute, value);
                
                Quaternion rotation; //If player uses action on himself don't need rotation
                if(!user.transform.CompareTag("Player"))
                    rotation = Quaternion.LookRotation(-Camera.main.transform.forward);
                else
                    rotation = Quaternion.LookRotation(Camera.main.transform.forward);

                effects.ForEach(f => 
                    Gamemaster.Instance.effectsManager.AddEffect(f.gameObject.name, user.transform.position + user.DamageEffectAnchor, rotation, f.delay, user)
                );
                break;
            case ETarget.Other:
                if(other != null) 
                {
                    other.CharacterSheet.Change(derivedAttribute, value);
                    effects.ForEach(f => 
                        Gamemaster.Instance.effectsManager.AddEffect(f.gameObject.name, other.transform.position + other.DamageEffectAnchor, Quaternion.LookRotation(Camera.main.transform.forward), f.delay)
                    );
                }
                break;
            default:
                user.CharacterSheet.Change(derivedAttribute, value);
                break;
        }

        base.Execute(user, other);
    }

    private bool HasEffectsScript(GameObject go)
    {
        return go != null && go.GetComponent<ParticleFx>() != null;
    }
}