using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerDashingState : PlayerGroundedState
{
    private float startTime;

    private int consecutiveDashesUsed;

    private bool shouldKeepRotating;

    public PlayerDashingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.ReusableData.MovementSpeedModifier = groundedData.DashData.SpeedModifier;
        stateMachine.Player.currentCharacter.animator.SetLayerWeight(3, 0);
        base.Enter();

        EffectActive(stateMachine.Player.dashEffect, true);

        StartAnimation(stateMachine.Player.AnimationData.DashParameterHash);

        stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StrongForce;

        stateMachine.ReusableData.RotationData = groundedData.DashData.RotationData;

        Dash();

        shouldKeepRotating = stateMachine.ReusableData.MovementInput != Vector2.zero;

        UpdateConsecutiveDashes();

        startTime = Time.time;
    }

    public override void Exit()
    {
        stateMachine.Player.currentCharacter.animator.SetLayerWeight(3, 1);
        base.Exit();

        EffectActive(stateMachine.Player.dashEffect, false);

        StopAnimation(stateMachine.Player.AnimationData.DashParameterHash);

        SetBaseRotationData();
    }

    public override void PhysicsUpdate()
    {
        //base.PhysicsUpdate();

        //if (!shouldKeepRotating)
        //{
        //    return;
        //}

        Float();

        //RotateTowardsTargetRotation();
    }

    private float SetSlopeSpeedModifierOnAngle(float angle)
    {
        float slopeSpeedModifier = groundedData.SlopeSpeedAngles.Evaluate(angle);

        if (stateMachine.ReusableData.MovementOnSlopesSpeedModifier != slopeSpeedModifier)
        {
            stateMachine.ReusableData.MovementOnSlopesSpeedModifier = slopeSpeedModifier;

            UpdateCameraRecenteringState(stateMachine.ReusableData.MovementInput);
        }

        return slopeSpeedModifier;
    }

    private void Float()
    {
        Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Player.ResizableCapsuleCollider.CapsuleColliderData.Collider.bounds.center;

        Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

        if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, stateMachine.Player.ResizableCapsuleCollider.SlopeData.FloatRayDistance, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
        {
            float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

            float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);

            if (slopeSpeedModifier == 0f)
            {
                return;
            }

            float distanceToFloatingPoint = stateMachine.Player.ResizableCapsuleCollider.CapsuleColliderData.ColliderCenterInLocalSpace.y * stateMachine.Player.transform.localScale.y - hit.distance;

            if (distanceToFloatingPoint == 0f)
            {
                return;
            }

            float amountToLift = distanceToFloatingPoint * stateMachine.Player.ResizableCapsuleCollider.SlopeData.StepReachForce - GetPlayerVerticalVelocity().y;

            Vector3 liftForce = new Vector3(0f, amountToLift, 0f);

            stateMachine.Player.Rigidbody.AddForce(liftForce, ForceMode.VelocityChange);
        }
    }

    public override void OnAnimationTransitionEvent()
    {
        if (stateMachine.ReusableData.MovementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.HardStoppingState);

            return;
        }

        stateMachine.ChangeState(stateMachine.SprintingState);
    }

    protected override void AddInputActionsCallbacks()
    {
        base.AddInputActionsCallbacks();

        stateMachine.Player.Input.PlayerActions.Movement.performed += OnMovementPerformed;

    }

    protected override void RemoveInputActionsCallbacks()
    {
        base.RemoveInputActionsCallbacks();

        stateMachine.Player.Input.PlayerActions.Movement.performed -= OnMovementPerformed;
    }

    protected override void OnMovementPerformed(InputAction.CallbackContext context)
    {
        base.OnMovementPerformed(context);

        shouldKeepRotating = true;
    }

    private void Dash()
    {
        Vector3 dashDirection = stateMachine.Player.transform.forward;

        dashDirection.y = 0f;

        UpdateTargetRotation(dashDirection, false);

        if (stateMachine.ReusableData.MovementInput != Vector2.zero)
        {
            UpdateTargetRotation(GetMovementInputDirection());

            dashDirection = GetTargetRotationDirection(stateMachine.ReusableData.CurrentTargetRotation.y);
        }

        stateMachine.Player.Rigidbody.velocity = dashDirection * GetMovementSpeed(false);
        stateMachine.Player.transform.forward = stateMachine.Player.Rigidbody.velocity.normalized;
    }

    private void UpdateConsecutiveDashes()
    {
        if (!IsConsecutive())
        {
            consecutiveDashesUsed = 0;
        }

        ++consecutiveDashesUsed;

        if (consecutiveDashesUsed == groundedData.DashData.ConsecutiveDashesLimitAmount)
        {
            consecutiveDashesUsed = 0;

            stateMachine.Player.Input.DisableActionFor(stateMachine.Player.Input.PlayerActions.Dash, groundedData.DashData.DashLimitReachedCooldown);
        }
    }

    private bool IsConsecutive()
    {
        return Time.time < startTime + groundedData.DashData.TimeToBeConsideredConsecutive;
    }

    protected override void OnDashStarted(InputAction.CallbackContext context)
    {
    }
}

