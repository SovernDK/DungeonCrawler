using System;
using System.Collections.Generic;
using Crawler;
using Crawler.Actors;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Sheet 
{
    [Serializable]
    public class CharacterState
    {
        [SerializeField] [ReadOnly]
        private int id;

        [SerializeField] [BoxGroup("General")] [PreviewField] [HideLabel]
        private Sprite icon;

        [SerializeField] [BoxGroup("General")]
        private string name;

        [SerializeField] [BoxGroup("General")]
        private EExecuteWhen executeWhen;

        [SerializeField] [BoxGroup("General")]
        private int turnsToEnd;
        private int currentTurnsToEnd;

        [SerializeField] [BoxGroup("Effects")]
        private List<EffectPrototype> effects;

        [SerializeField] [InlineEditor]
        private List<Action> actions;
    
        public string Name { get => name; set => name = value; }
        public int Id { get => id; set => id = value; }
        public List<Action> Actions { get => actions; set => actions = value; }
        public EExecuteWhen ExecuteWhen { get => executeWhen; set => executeWhen = value; }
        public Sprite Icon { get => icon; set => icon = value; }
        public int CurrentTurnsToEnd { get => currentTurnsToEnd; }

        public CharacterState(string name)
        {
            this.name = name;
        }

        public void Execute(Actor actor)
        {
            actions.ForEach(action => action.Execute(actor, actor));
            currentTurnsToEnd--;
            effects.ForEach(f => {
                Gamemaster.Instance.effectsManager.AddEffect(f.gameObject.name, actor.transform.position + actor.DamageEffectAnchor, Quaternion.LookRotation(Camera.main.transform.forward), f.delay, actor);
            });
        }

        public CharacterState Copy()
        {
            CharacterState copy = new CharacterState(name) 
            {
                id = id,
                icon = icon,
                executeWhen = executeWhen,
                turnsToEnd = turnsToEnd,
                currentTurnsToEnd = turnsToEnd,
                actions = actions,
                effects = effects
            };

            return copy;
        }
    }

    public enum EExecuteWhen
    {
        StartOfTurn, EndOfTurn, OneTime
    }
}
