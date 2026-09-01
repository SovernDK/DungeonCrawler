using System.Collections;
using Crawler.Actors;
using Crawler.Commands;

public class ActorSpellCastCommand : ActorCommand
{
    public ActorSpellCastCommand(MainContext context, Actor actor, string name) : base(context, actor, name)
    {
        
    }

    public override IEnumerator Execute()
    {
        actor.CurrentSpell.PlayEffects(actor.Target);
        actor.ActionPoints -= actor.CurrentSpell.ActionCost;

        yield return actor.ActorContext.PlaySpellCasting();
        
        actor.CharacterSheet.Change(Sheet.EDerivedAttribute.Mana, -actor.CurrentSpell.ManaCost);
        actor.CurrentSpell.Cast(actor, actor.Target);
        actor.CurrentSpell = null;
    }
}
