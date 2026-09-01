using Crawler;
using Crawler.Actors;
using Sirenix.OdinInspector;
using UnityEngine;

public class FloorSpikes : Trap
{
    public Transform spikes;
    
    [InlineProperty]
    public EffectPrototype activated;

    public override void Activate(Actor actor)
    {
        base.Activate(actor);
        Gamemaster.Instance.effectsManager.AddEffect(activated.gameObject.name, transform.position, Quaternion.identity, activated.delay);
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }
}
