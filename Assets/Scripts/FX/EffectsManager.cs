using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using Crawler.Actors;
using System.Linq;

namespace Crawler
{
    [Serializable]
    public class EffectsManager : MonoBehaviour
    {
        private Queue<Effect> effectQueue = new Queue<Effect>();

        private void Start()
        {
            StartCoroutine(ProcessEffectQueue());
        }

        private IEnumerator ProcessEffectQueue()
        {
            while (true)
            {
                if (effectQueue.Count > 0)
                {
                    Effect currentEffect = effectQueue.Dequeue();
                    yield return new WaitForSeconds(currentEffect.delay);
                    GameObject effect = PoolManager.Instance.SpawnFromPool(currentEffect.effectName, currentEffect.position, currentEffect.rotation);
                    effect.GetComponent<IEffect>().Follow = currentEffect.follow;
                }
                else
                {
                    yield return null;
                }
            }
        }

        public void AddEffect(string effectName, Vector3 position, Quaternion rotation, float delay, Actor follow = null)
        {
            effectQueue.Enqueue(new Effect(effectName, position, rotation, delay, follow));
        }

        public void AddEffect(IEffect fx, Vector3 position, Quaternion rotation, float delay, Actor follow = null)
        {
            if(fx.OneAtTime && effectQueue.ToList().Find(eff => eff.effectName == fx.EffectName) != null)
            {
                return;
            }

            effectQueue.Enqueue(new Effect(fx.EffectName, position, rotation, delay, follow));
        }
    }

    [Serializable]
    public class Effect
    {
        public string effectName;
        public Vector3 position;
        public Quaternion rotation;
        public float delay;
        public Actor follow;

        public Effect(string name, Vector3 pos, Quaternion rot, float del, Actor fol = null)
        {
            effectName = name;
            position = pos;
            rotation = rot;
            delay = del;
            follow = fol;
        }
    }

    [Serializable]
    public class EffectPrototype
    {
        [HideLabel] [HorizontalGroup("General")]
        public GameObject gameObject;
        
        [HideLabel] [HorizontalGroup("General")]
        public float delay;
    }
}
