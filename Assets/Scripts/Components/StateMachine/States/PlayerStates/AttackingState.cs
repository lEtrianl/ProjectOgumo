using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingState : MovableState
{
    public AttackingState(Player player, IStateMachine stateMachine, IPlayerInput playerInput, PlayerInterstateData playerInterstateData) : base(player, stateMachine, playerInput, playerInterstateData) { }

    public override void Enter()
    {
        base.Enter();

        player.Weapon.StartAttack();
        player.EffectManager.AddEffect(player.SlowdownDuringAttack);
    }

    public override void Exit()
    {
        base.Exit();

        player.Weapon.BreakAttack();
        player.EffectManager.RemoveEffect(player.SlowdownDuringAttack);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (player.Weapon.IsAttacking == false)
        {
            stateMachine.ChangeState(player.Standing);
        }
    }

    protected override void OnAttack(eActionPhase actionPhase)
    {
        if (actionPhase == eActionPhase.Started)
        {
            player.Weapon.StartAttack();
        }
    }
}
