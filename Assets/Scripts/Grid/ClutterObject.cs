using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ClutterObject : MonoBehaviour, ISaveableObject, IPoolable
{
    public string Name => Utility.GetObjectNameWithoutSuffix(gameObject);

    public GameObject Prefab => gameObject;

    public int AmountToPool => 10;

    public bool ShouldExpand => true;

    public string GetObjectId()
    {
        return GetInstanceID().ToString();
    }

    public void Load(SaveableObject saveData)
    {
        
    }

    public void OnObjectDisbaled()
    {

    }

    public void OnObjectEnabled()
    {

    }

    public void OnSpawned()
    {

    }

    public SaveableObject Save()
    {
        return new SaveableObject
        {
            name = Name,
            objectId = GetInstanceID().ToString(),
            type = GetType().ToString(),
            position = transform.position,
            rotation = transform.rotation,
            data = ""
        };
    }
}
