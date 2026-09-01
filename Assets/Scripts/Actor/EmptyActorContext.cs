using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyActorContext : MonoBehaviour, IActorContext
{
    public bool Done { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public string ActorName { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public bool Alive => throw new System.NotImplementedException();

    public MainContext MainContext => throw new System.NotImplementedException();

    public bool Busy { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
}
