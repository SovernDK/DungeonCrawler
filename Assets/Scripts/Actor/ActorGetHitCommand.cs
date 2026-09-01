using System.Collections;
using Crawler.Actors;
using Crawler.Commands;

public class ActorGetHitCommand : ActorCommand
{
    public ActorGetHitCommand(MainContext context, Actor actor, string name) : base(context, actor, name)
    {
        
    }

    public override IEnumerator Execute()
    {
        yield return actor.ActorContext.PlayGetHit(); 
    }
}
