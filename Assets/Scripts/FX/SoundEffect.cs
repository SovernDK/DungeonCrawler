using System.Collections;
using System.Collections.Generic;
using Crawler.Actors;
using Sirenix.OdinInspector;
using UnityEngine;

public class SoundEffect : MonoBehaviour, IPoolable, IEffect
{
    [SerializeField] 
    private AudioSource source;

    [SerializeField] 
    private float time;

    [SerializeField] 
    private bool randomizePitch;
    
    [SerializeField] [ShowIf("@randomizePitch == true")]
    private Vector2 pitchRange;

    [SerializeField] 
    private bool oneAtTime;
    
    private Actor follow;
    private float _time;

    public string Name => Utility.GetObjectNameWithoutSuffix(gameObject);

    public GameObject Prefab => gameObject;

    public int AmountToPool => 2;

    public bool ShouldExpand => true;

    public Actor Follow { get => follow; set => follow = value; }
    public string EffectName { get => Name; }
    public bool OneAtTime { get => oneAtTime; set => oneAtTime = value; }

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
        if(randomizePitch)
            source.pitch = Random.Range(pitchRange.x, pitchRange.y); 
            
        source.Play(0);   
        _time = 0; 
    }

    private void Update() 
    {
        if(gameObject.activeSelf)
        {
            _time += Time.deltaTime;
            
            if(Follow != null)
                transform.position = Follow.transform.position + Follow.DamageEffectAnchor;

            if(_time > time)
            {
                transform.position = Vector3.zero;
                gameObject.SetActive(false);
            }
        }    
    }
}
