using Crawler;
using Crawler.FSM;
using Unity.VisualScripting;

public class PlayerStateLoot : PlayerState
{
    private Player player;
    private ContainerInteraction chestInteraction;
    private Lootable lootable;

    public PlayerStateLoot(Player player)
    {
        this.player = player;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        lootable = player.CurrentInteractable as Lootable;
        Gamemaster.Instance.gamePreferences.SetCursor("Default");
        lootable.Opened();
    }
    
    public override void OnUpdate()
    {
        base.OnUpdate();

        if(Gamemaster.Instance.inputManager.Back)
        {
            player.Fsm.SetState((int) EPlayerState.Actor);
            (Gamemaster.Instance.context as MainContext).mainScene.infoPanel.gameObject.SetActive(false);
        }

        if(!(Gamemaster.Instance.context as MainContext).mainScene.IsWindowOpen("OtherInventory"))
        {
            player.Fsm.SetState((int) EPlayerState.Actor);
            (Gamemaster.Instance.context as MainContext).mainScene.infoPanel.gameObject.SetActive(false);
        }
    }

    public override void OnExit()
    {
        base.OnExit();

        lootable.Closed();
    }
}
