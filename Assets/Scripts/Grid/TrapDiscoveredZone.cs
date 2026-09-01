using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDiscoveredZone : Interactable
{
    private Trap trap;

    public override void OnInit()
    {
        base.OnInit();
        trap = GetComponentInParent<Trap>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnInteractedWith(Player player)
    {
        base.OnInteractedWith(player);

        if(player.Actor.InventoryController.Inventory.ContainsItem(19)) //Trap disarming kit
        {
            trap.Deactivate();
        }       
        else
        {
            Gamemaster.Instance.gamelog.NewEntry(player.Actor, null, "Disarming traps require Trap Disarming Kit");
        } 
    }
}
