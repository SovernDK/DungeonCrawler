using Crawler.Actors;
using Crawler.FSM;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateAim : PlayerState
{
    private Player player;

    public PlayerStateAim(Player player)
    {
        this.player = player;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Gamemaster.Instance.gamePreferences.SetCursor("SpellDisabled");
        Gamemaster.Instance.inputManager.GameInputs.PlayerControl.Confirm.performed += CastSpell;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        player.Inputs();
        player.Interactor(2);

        if(Gamemaster.Instance.inputManager.Back)
        {
            player.Fsm.SetState((int) EPlayerState.Actor);
        }

        if(player.CurrentInteractable != null && 
            player.CurrentInteractable.TryGetComponent(out IActorContext actor) && 
            Gamemaster.Instance.gamePreferences.CursorType != "Spell")
        {
            Gamemaster.Instance.gamePreferences.SetCursor("Spell");
        }
        else if(player.CurrentInteractable == null && Gamemaster.Instance.gamePreferences.CursorType != "SpellDisabled")
        {
            Gamemaster.Instance.gamePreferences.SetCursor("SpellDisabled");
        }
    }

    private void CastSpell(InputAction.CallbackContext context)
    {
        if(player.CurrentInteractable != null && player.CurrentInteractable.TryGetComponent(out Actor other))
        {
            player.Actor.Target = other;
            player.Actor.CurrentSpell = player.CurrentSpell;
            (Gamemaster.Instance.context as MainContext).gameFlow.AddCommand("SpellCast", player.Actor);
            player.Fsm.isActing = true;
            // player.Actor.CurrentSpell = null;
            player.Fsm.SetState((int) EPlayerState.Actor);
        }
        else 
        {
            player.Actor.CurrentSpell = null;
            player.Fsm.SetState((int) EPlayerState.Actor);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        Gamemaster.Instance.inputManager.GameInputs.PlayerControl.Confirm.performed -= CastSpell;
    }
}
