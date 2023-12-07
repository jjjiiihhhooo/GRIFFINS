using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace genshin
{
    public class PlayerLightLandingState : PlayerLandingState
    {
        public PlayerLightLandingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            if(stateMachine.GetPreviousState() != typeof(PlayerDashingState))
            {
                stateMachine.ReusableData.MovementSpeedModifier = 0;
            }

            base.Enter();

            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StationaryForce;

            if (stateMachine.GetPreviousState() != typeof(PlayerDashingState))
            {
                ResetVelocity();
            }

        }

        public override void Update()
        {
            base.Update();

            if (stateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                return;
            }

            OnMove();
        }

        public override void PhysicsUpdate()
        {
            if (stateMachine.GetPreviousState() != typeof(PlayerDashingState))
            {
                base.PhysicsUpdate();

                if (!IsMovingHorizontally())
                {
                    return;
                }

                ResetVelocity();
            }
        }

        public override void OnAnimationTransitionEvent()
        {
            if (stateMachine.GetPreviousState() != typeof(PlayerDashingState))
            {
                stateMachine.ChangeState(stateMachine.IdlingState);
            }
        }
    }
}
