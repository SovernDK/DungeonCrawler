using Crawler.Actors;
using Sheet;
using UnityEngine;
using System.Collections.Generic;
using Crawler;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "CharacterStatesAction", menuName = "Actions/CharacterStatesAction")]
public class CharacterStatesAction : Action
{
    [SerializeField] [BoxGroup("States")] 
    protected List<ApplyStates> applyStates;

    [SerializeField] [BoxGroup("States")] 
    protected List<ApplyStates> removeStates;


    public override void Execute(Actor user, Actor other = null)
    {
        base.Execute(user, other);

        applyStates.ForEach(state => 
        {
            int diceThrow = Random.Range(0, 101);
            if(target == ETarget.Self)
            {
                if(diceThrow <= state.chance) user.CharacterSheet.ApplyState(state.card.State);
            }
            else
            {
                if(diceThrow <= state.chance) other.CharacterSheet.ApplyState(state.card.State);
            }        
        });

        removeStates.ForEach(state => 
        {
            if(target == ETarget.Self)
            {
                user.CharacterSheet.RemoveState(state.card.State);
            }
            else
            {
                other.CharacterSheet.RemoveState(state.card.State);
            }        
        });
    }

    private bool HasEffectsScript(GameObject go)
    {
        return go != null && go.GetComponent<ParticleFx>() != null;
    }
}