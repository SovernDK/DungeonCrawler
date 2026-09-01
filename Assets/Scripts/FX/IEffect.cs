using Crawler.Actors;
using UnityEngine;

public interface IEffect
{
    public string EffectName { get; }
    public bool OneAtTime { get; }
    public Actor Follow { get; set; }
}
