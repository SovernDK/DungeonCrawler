using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable
{
    #region IPoolable Properties
    string Name { get; }
    GameObject Prefab { get; }
    int AmountToPool { get; }
    bool ShouldExpand { get; }
    #endregion

    #region IPoolable Members
    public void OnObjectEnabled();
    public void OnObjectDisbaled();
    public void OnSpawned();
    #endregion
}
