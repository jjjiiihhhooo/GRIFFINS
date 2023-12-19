using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace genshin
{
    public class TornadoState : PlayerSkillState
    {
        public TornadoState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            stateMachine.Player.isSkill = true;
            StartAnimation(stateMachine.Player.AnimationData.TornadoParameterHash);
            stateMachine.Player.Rigidbody.velocity = Vector3.zero;
            stateMachine.Player.Rigidbody.useGravity = false;
            stateMachine.Player.pm.bounceCombine = PhysicMaterialCombine.Minimum;
            stateMachine.Player.transform.forward = stateMachine.Player.MainCameraTransform.forward;
        }

        public override void Exit()
        {
            stateMachine.Player.isSkill = false;
            base.Exit();
            StopAnimation(stateMachine.Player.AnimationData.TornadoParameterHash);
        }

        public override void PhysicsUpdate()
        {

        }

        public void Tornado()
        {
            stateMachine.Player.Rigidbody.useGravity = true;
            stateMachine.Player.tornadoSkillObject.transform.position = new Vector3(stateMachine.Player.transform.position.x, stateMachine.Player.transform.position.y + 1f, stateMachine.Player.transform.position.z);
            Vector3 dir = stateMachine.Player.MainCameraTransform.forward;
            stateMachine.Player.tornadoSkillObject.GetComponent<TornadoEvent>().TornadoEffect.SetActive(false);
            stateMachine.Player.tornadoSkillObject.SetActive(false);
            stateMachine.Player.tornadoSkillObject.SetActive(true);

            if (!stateMachine.Player.isGround) stateMachine.ChangeState(stateMachine.FallingState);
            else if (stateMachine.ReusableData.MovementInput != Vector2.zero) stateMachine.ChangeState(stateMachine.RunningState);
            else stateMachine.ChangeState(stateMachine.IdlingState);

        }


        public override void OnAnimationTransitionEvent()
        {
            Tornado();
        }


    }
}
