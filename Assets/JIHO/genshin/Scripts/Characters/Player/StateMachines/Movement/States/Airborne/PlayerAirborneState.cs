using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAirborneState : PlayerMovementState
{
    public PlayerAirborneState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    //public override void Enter()
    //{
    //    base.Enter();

    //    StartAnimation(stateMachine.Player.AnimationData.AirborneParameterHash);

    //    ResetSprintState();
    //}

    //public override void Exit()
    //{
    //    base.Exit();

    //    StopAnimation(stateMachine.Player.AnimationData.AirborneParameterHash);
    //}

    protected override void AddInputActionsCallbacks()
    {
        if (GameManager.Instance.dialogueManager.IsChat) return;

        //stateMachine.Player.Input.PlayerActions.Dash.started += OnDashStarted;
    }

    protected override void RemoveInputActionsCallbacks()
    {
        //stateMachine.Player.Input.PlayerActions.Dash.started -= OnDashStarted;
    }



    protected virtual void OnDashStarted(InputAction.CallbackContext context)
    {
        if (Player.Instance.skillData.isHand) return;
        if (!GameManager.Instance.staminaManager.ChechStamina(20f)) return;
        if (!GameManager.Instance.coolTimeManager.CoolCheck("Dash")) return;
        GameManager.Instance.staminaManager.MinusStamina(20f);
        GameManager.Instance.coolTimeManager.GetCoolTime("Dash");
        stateMachine.ChangeState(stateMachine.AirDashingState);
    }

    //protected virtual void ResetSprintState()
    //{
    //    stateMachine.ReusableData.ShouldSprint = false;
    //}

    //protected override void OnContactWithGround(Collider collider)
    //{
    //    stateMachine.ChangeState(stateMachine.LightLandingState);
    //}


    public override void Enter()
    {
        base.Enter();

        StartAnimation(stateMachine.Player.AnimationData.AirborneParameterHash);

        ResetSprintState();
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.Player.AnimationData.AirborneParameterHash);
    }

    protected virtual void ResetSprintState()
    {
        stateMachine.ReusableData.ShouldSprint = false;
    }

    protected override void OnContactWithGround(Collider collider)
    {
        stateMachine.ChangeState(stateMachine.LightLandingState);
    }

}

