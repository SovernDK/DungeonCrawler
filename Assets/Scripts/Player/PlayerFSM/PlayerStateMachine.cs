using System;
using System.Collections;

namespace Crawler.FSM 
{
    [Serializable]
    public class PlayerStateMachine : FiniteStateMachine<PlayerState, Player>
    {
        public delegate void StateChangedDelegate(PlayerState playerState);
        public event StateChangedDelegate onStateChanged;

        public EPlayerState currentStateId;

        public bool isActing;

        public override void Init(Player player)
        {
            base.Init(player);

            states = new PlayerState[] 
            {
                new PlayerStateActor(player),
                new PlayerStateLoot(player),
                new PlayerStateAim(player),
            };

            CurrentState = states[0];
        }

        public IEnumerator Update()
        {
            isActing = false;

            while (!isActing)
            {
                CurrentState.OnUpdate();
                yield return null;
            }
        }

        public override void SetState(int id)
        {
            base.SetState(id);

            CurrentState.OnExit();
            CurrentState = states[id];
            currentStateId = (EPlayerState) id;
            CurrentState.OnEnter();
            onStateChanged?.Invoke(CurrentState);
        }
    }

    public enum EPlayerState 
    {
        Actor, Loot, Aim
    }
}