using Crawler.Actors;
using UnityEngine;

public class ParticleFx : MonoBehaviour, IPoolable, IEffect
{
    [SerializeField] 
    private ParticleSystem particle;

    [SerializeField] 
    private float time;

    [SerializeField] 
    private Actor follow;
    private float _time;

    #region IPoolable Properties
    public string Name => Utility.GetObjectNameWithoutSuffix(gameObject);

    public GameObject Prefab => gameObject;

    public int AmountToPool => 2;

    public bool ShouldExpand => true;
    #endregion
    public Actor Follow { get => follow; set => follow = value; }

    public string EffectName => Name;

    public bool OneAtTime => false;

    public void OnObjectDisbaled()
    {
    }

    public void OnObjectEnabled()
    {
    }

    public void OnSpawned()
    {
    }

    private void OnEnable() 
    {
        particle.Play();   
        _time = 0; 
    }

    private void Update() 
    {
        if(gameObject.activeSelf)
        {
            _time += Time.deltaTime;

            if(Follow != null)
            {
                transform.position = Follow.transform.position + Follow.DamageEffectAnchor;
            }
                
            if(_time > time)
            {
                transform.position = Vector3.zero;
                gameObject.SetActive(false);
            }
        }    
    }
}
