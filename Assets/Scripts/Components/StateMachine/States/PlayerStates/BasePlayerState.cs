using UnityEngine;

public abstract class BasePlayerState : IState
{
    protected Player player;
    protected IPlayerInput playerInput;
    protected IStateMachine stateMachine;

    protected PlayerInterstateData playerInterstateData;

    //protected static bool haveToTurn;
    //protected static bool isMoving;
    //protected static int abilityNumberToCast;
    //protected static int previousAbilityNumber;

    public BasePlayerState(Player player, IStateMachine stateMachine, IPlayerInput playerInput, PlayerInterstateData playerInterstateData)
    {
        this.player = player;
        this.playerInput = playerInput;
        this.stateMachine = stateMachine;
        this.playerInterstateData = playerInterstateData;
    }

    public virtual void Enter()
    {
        playerInput.MoveEvent.AddListener(OnMove);
        playerInput.StopEvent.AddListener(OnStop);
        playerInput.CrouchEvent.AddListener(OnCrouch);
        playerInput.JumpEvent.AddListener(OnJump);
        playerInput.AttackEvent.AddListener(OnAttack);
        playerInput.RollEvent.AddListener(OnRoll);
        playerInput.ParryEvent.AddListener(OnParry);
        playerInput.InteractEvent.AddListener(OnInteract);
        playerInput.ClimbUpEvent.AddListener(OnClimbUp);
        playerInput.AbilityEvent.AddListener(OnAbilityUse);
        playerInput.ChangeAbilityLayoutEvent.AddListener(OnChangeAbilityLayout);

        player.EffectManager.EffectEvent.AddListener(OnStun);
        player.DeathManager.DeathEvent.AddListener(OnDeath);
    }

    public virtual void Exit()
    {
        playerInput.MoveEvent.RemoveListener(OnMove);
        playerInput.StopEvent.RemoveListener(OnStop);
        playerInput.CrouchEvent.RemoveListener(OnCrouch);
        playerInput.JumpEvent.RemoveListener(OnJump);
        playerInput.AttackEvent.RemoveListener(OnAttack);
        playerInput.RollEvent.RemoveListener(OnRoll);
        playerInput.ParryEvent.RemoveListener(OnParry);
        playerInput.InteractEvent.RemoveListener(OnInteract);
        playerInput.ClimbUpEvent.RemoveListener(OnClimbUp);
        playerInput.AbilityEvent.RemoveListener(OnAbilityUse);
        playerInput.ChangeAbilityLayoutEvent.RemoveListener(OnChangeAbilityLayout);

        player.EffectManager.EffectEvent.RemoveListener(OnStun);
        player.DeathManager.DeathEvent.RemoveListener(OnDeath);
    }

    public virtual void LogicUpdate()
    {
        if (player.Climb.CanClimb == true && (playerInterstateData.climbUp || (playerInterstateData.climbDown && player.Gravity.IsGrounded == false)))
        {
            stateMachine.ChangeState(player.Climbing);
        }
    }

    public virtual void PhysicsUpdate()
    {

    }

    protected virtual void OnMove(eDirection direction)
    {
        playerInterstateData.haveToTurn = direction == player.Turning.Direction ? false : true;

        playerInterstateData.isMoving = true;
    }

    protected virtual void OnStop()
    {
        playerInterstateData.isMoving = false;
    }

    protected virtual void OnCrouch(eActionPhase actionPhase)
    {
        //if (actionPhase == eActionPhase.Started)
        //{
        //    stateMachine.ChangeState(player.Crouching);
        //}

        switch (actionPhase)
        {
            case eActionPhase.Started:
                playerInterstateData.climbDown = true;
                break;

            case eActionPhase.Canceled:
                playerInterstateData.climbDown = false;
                break;
        }
    }

    protected virtual void OnJump(eActionPhase actionPhase)
    {
        if (actionPhase == eActionPhase.Started && player.Jump.CanJump == true)
        {
            stateMachine.ChangeState(player.Jumping);
        }
    }

    protected virtual void OnRoll(eActionPhase actionPhase)
    {
        if (actionPhase == eActionPhase.Started && player.Roll.CanRoll == true)
        {
            stateMachine.ChangeState(player.Rolling);
        }
    }

    protected virtual void OnParry(eActionPhase actionPhase)
    {
        if (actionPhase == eActionPhase.Started && player.Parry.CanParry == true)
        {
            stateMachine.ChangeState(player.Parrying);
        }
    }

    protected virtual void OnAttack(eActionPhase actionPhase)
    {
        if (actionPhase == eActionPhase.Started)
        {
            stateMachine.ChangeState(player.Attacking);
        }
    }

    protected virtual void OnInteract(eActionPhase actionPhase)
    {
        if (actionPhase == eActionPhase.Started && player.Interact.CanInteract == true)
        {
            stateMachine.ChangeState(player.Interacting);
        }
    }

    protected virtual void OnClimbUp(eActionPhase actionPhase)
    {
        switch (actionPhase)
        {
            case eActionPhase.Started:
                playerInterstateData.climbUp = true;
                break;

            case eActionPhase.Canceled:
                playerInterstateData.climbUp = false;
                break;
        }
    }

    protected virtual void OnAbilityUse(eActionPhase actionPhase, int abilityNumber)
    {
        if (actionPhase == eActionPhase.Started && player.AbilityManager.CanCastAbility(abilityNumber))
        {
            playerInterstateData.previousAbilityNumber = playerInterstateData.abilityNumberToCast;

            if (player.AbilityManager.IsPerforming(playerInterstateData.previousAbilityNumber))
            {
                playerInterstateData.breakPreviousAbility = true;
            }

            playerInterstateData.abilityNumberToCast = abilityNumber;
            stateMachine.ChangeState(player.CastingAbility);
        }
    }

    protected virtual void OnChangeAbilityLayout(eActionPhase actionPhase)
    {
        if (actionPhase == eActionPhase.Started)
        {
            player.AbilityManager.SwitchAbilityLayout();
        }
    }

    protected virtual void OnStun(eEffectType effectType, eEffectStatus effectStatus)
    {
        if (effectType == eEffectType.Stun && effectStatus == eEffectStatus.Added)
        {
            stateMachine.ChangeState(player.Stunned);
        }
    }

    protected virtual void OnDeath()
    {
        stateMachine.ChangeState(player.Dying);
    }
}
