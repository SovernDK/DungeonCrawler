using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFSMState
{
    public void OnEnter();
    public void OnUpdate();
    public void OnExit();
}
