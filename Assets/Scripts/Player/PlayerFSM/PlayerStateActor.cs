using System.Collections;
using System.Collections.Generic;
using Crawler.Actors;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateActor : PlayerState
{
    private Player player;

    public PlayerStateActor(Player player)
    {
        this.player = player;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        
        Gamemaster.Instance.gamePreferences.SetCursor("Default");
        Gamemaster.Instance.inputManager.GameInputs.PlayerControl.Confirm.performed += OnConfirm;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        player.Inputs();
        player.Interactor(1);

        if(player.CurrentInteractable != null && player.CurrentInteractable.gameObject.CompareTag("Enemy") && Gamemaster.Instance.gamePreferences.CursorType != "Attack")
        {
            Gamemaster.Instance.gamePreferences.SetCursor("Attack");
        }
        else if(player.CurrentInteractable != null && player.CurrentInteractable.gameObject.CompareTag("Trap") && Gamemaster.Instance.gamePreferences.CursorType != "Disarm")
        {
            Gamemaster.Instance.gamePreferences.SetCursor("Disarm");
        }
        else if(player.CurrentInteractable != null && player.CurrentInteractable.gameObject.CompareTag("Container") && Gamemaster.Instance.gamePreferences.CursorType != "Open")
        {
            Gamemaster.Instance.gamePreferences.SetCursor("Open");
        }
        else if(player.CurrentInteractable != null && player.CurrentInteractable.gameObject.CompareTag("Torch") && Gamemaster.Instance.gamePreferences.CursorType != "Torch")
        {
            Gamemaster.Instance.gamePreferences.SetCursor("Torch");
        }
        else if(player.CurrentInteractable == null && Gamemaster.Instance.gamePreferences.CursorType != "Default")
        {
            Gamemaster.Instance.gamePreferences.SetCursor("Default");
        }
    }

    private void OnConfirm(InputAction.CallbackContext context)
    {
        if (player.CurrentInteractable && player.CurrentInteractable.TryGetComponent(out Enemy enemy) && !player.Fsm.isActing)
        {
            player.Actor.Target = enemy.actor;
            player.MainContext.gameFlow.AddCommand("Attack", player.Actor);
            player.Fsm.isActing = true;
        }
        else if(player.CurrentInteractable)
        {
            player.CurrentInteractable.OnInteractedWith(player);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        
        Gamemaster.Instance.inputManager.GameInputs.PlayerControl.Confirm.performed -= OnConfirm;
    }
}
