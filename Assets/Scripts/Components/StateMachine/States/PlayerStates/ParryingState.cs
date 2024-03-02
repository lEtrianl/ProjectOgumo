using UnityEngine;

public class ParryingState : TurnableState, IState
{
    public ParryingState(Player player, IStateMachine stateMachine, IPlayerInput playerInput, PlayerInterstateData playerInterstateData) : base(player, stateMachine, playerInput, playerInterstateData) { }

    public override void Enter()
    {
        base.Enter();

        player.Parry.StartParry();
    }

    public override void Exit()
    {
        base.Exit();

        player.Parry.BreakParry();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (player.Parry.IsParrying == false)
        {
            stateMachine.ChangeState(player.Standing);
        }
    }

    protected override void OnParry(eActionPhase actionPhase)
    {
        if (actionPhase == eActionPhase.Canceled)
        {
            stateMachine.ChangeState(player.Standing);
        }
    }
}
