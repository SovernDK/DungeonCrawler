using Crawler.Actors;
using UnityEngine;
using Sirenix.OdinInspector;
using Sheet;
using System;

public abstract class Action : ScriptableObject
{
    [SerializeField] [ReadOnly] 
    private int id;

    [SerializeField] [BoxGroup("General")] 
    protected ETarget target;

    [SerializeField] [BoxGroup("General")] 
    protected EElement element;

    public int Id { get => id; set => id = value; }
    public ETarget Target { get => target; set => target = value; }
    public EElement Element { get => element; set => element = value; }

    public virtual void Execute(Actor user, Actor other)
    {
       
    }

    public virtual void PlayEffects(Actor target)
    {
       
    }
}

public enum ETarget 
{
    Self, Other
}

public enum EElement 
{
    None, Fire, Wind, Water, Stabbing, Cutting, Blunt
}

[Serializable]
public class ApplyStates
{
    [InlineEditor] [HorizontalGroup("Apply")] [HideLabel]
    public CharacterStateCard card;

    [HorizontalGroup("Apply")] [Range(0, 100)] [HideLabel]
    public float chance;
}