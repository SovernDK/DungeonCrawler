using System.Collections;
using Crawler.Actors;
using Crawler.Commands;
using UnityEngine;

public class ActorAttackCommand : ActorCommand
{
    public ActorAttackCommand(MainContext context, Actor actor, string name) : base(context, actor, name)
    {
        
    }

    public override IEnumerator Execute()
    {
        EquipmentSlot slot = actor.InventoryController.Equipment.Slots[(int) EEquipmentSlot.RightHand];
        if(slot.Item.Id != -1) 
        {
            Item weapon = actor.InventoryController.Equipment.Slots[(int) EEquipmentSlot.RightHand].Item;
            weapon.PlayEffects(actor.Target);
            // if(actor.Target.ActorContext is Player)
            // {
            //     (actor.Target.ActorContext as Player).LookAtEnemy();
            // }
            
            yield return actor.ActorContext.PlayAttack(); 

            // if(actor.Target.ActorContext is Player)
            // {
            //     (actor.Target.ActorContext as Player).LookAtEnemy();
            // }

            weapon.Use(actor, actor.Target);
            actor.ActionPoints--;  
        }
    }
}
