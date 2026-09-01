using System;
using Crawler.Actors;
using Sirenix.OdinInspector;
using UnityEngine;

public class Trap : MonoBehaviour, ISaveableObject, IPoolable
{
    [SerializeField] [InlineProperty] [BoxGroup("Data")] [HideLabel]
    private TrapData trapData;

    [SerializeField] 
    private Action action;

    [SerializeField] 
    private Animator animator;

    protected bool loaded = true;
    protected bool tripped;
    
    public Transform discoveryZone;

    public TrapData TrapData { get => trapData; set => trapData = value; }

    #region IPoolable Properties
    public string Name => Utility.GetObjectNameWithoutSuffix(gameObject);
    public GameObject Prefab => gameObject;
    public int AmountToPool => 5;
    public bool ShouldExpand => true;
    #endregion

    public virtual void Activate(Actor actor)
    {
        tripped = true;
        loaded = false;

        action.Execute(actor, actor);
        animator.SetTrigger("activate");
    }

    public virtual void Deactivate()
    {
        loaded = false;
        tripped = true;
        discoveryZone.gameObject.SetActive(false);
    }

    internal void Discover()
    {
        if(!tripped && loaded)
        {
            discoveryZone.gameObject.SetActive(true);
        }
    }

    #region ISaveableObject Members
    public string GetObjectId()
    {
        return GetInstanceID().ToString();
    }

    public SaveableObject Save()
    {
        TrapData data = new TrapData()
        {
            discoveryLevel = trapData.discoveryLevel,
            coordinates = trapData.coordinates
        };

        return new SaveableObject
        {
            name = Name,
            objectId = GetInstanceID().ToString(),
            type = GetType().ToString(),
            position = transform.position,
            rotation = transform.rotation,
            data = JsonUtility.ToJson(data)
        };
    }

    public void Load(SaveableObject saveData)
    {
        if (saveData.data != null) 
        {
            trapData = JsonUtility.FromJson<TrapData>(saveData.data);
        }
    }
    #endregion

    #region IPoolable Members
    public void OnObjectEnabled()
    {
        
    }

    public void OnObjectDisbaled()
    {
        
    }

    public void OnSpawned()
    {
        
    }
    #endregion
}

[Serializable]
public class TrapData
{
    public Vector3Int coordinates;
    public int discoveryLevel;
}