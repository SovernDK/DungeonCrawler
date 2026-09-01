using System.Collections;
using System.Collections.Generic;
using Crawler.Actors;
using DG.Tweening;
using UnityEngine;


namespace Crawler.Commands
{
    public class ActorRotateCommand : ActorCommand
    {
        public float direction;
        public ActorRotateCommand(MainContext context, Actor actor, string name, float direction) : base(context, actor, name)
        {
            this.direction = direction;
        }

        public override IEnumerator Execute()
        {
            float newRotation = Mathf.Repeat(actor.Rotation + direction, 4);
            actor.Rotation = newRotation;

            yield return actor.ActorContext.PlayRotate(newRotation);
        }
    }
}