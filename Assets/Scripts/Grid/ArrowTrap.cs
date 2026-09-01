using Crawler.Actors;
using UnityEngine;

public class ArrowTrap : Trap
{
    public Transform arrowsBundle;

    public override void Activate(Actor actor)
    {
        if(loaded && !tripped)
        {
            discoveryZone.gameObject.SetActive(false);
            arrowsBundle.gameObject.SetActive(true);
            
            base.Activate(actor);
        }
    }
}
