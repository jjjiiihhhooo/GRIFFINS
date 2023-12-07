using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace genshin
{
    public class TornadoState : PlayerAirborneState
    {
        public TornadoState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation(stateMachine.Player.AnimationData.TornadoParameterHash, 1);
            stateMachine.Player.Rigidbody.velocity = Vector3.zero;
            stateMachine.Player.Rigidbody.useGravity = false;
            stateMachine.Player.pm.bounceCombine = PhysicMaterialCombine.Minimum;
            stateMachine.Player.transform.forward = stateMachine.Player.MainCameraTransform.forward;
        }

        public override void Exit()
        {
            base.Exit();

        }

        public override void PhysicsUpdate()
        {

            //RotateTowardsTargetRotation();
        }

        public void Tornado()
        {

        }

        private IEnumerator TornadoCor()
        {
            
            stateMachine.Player.tornadoSkillObject.transform.position = new Vector3(stateMachine.Player.transform.position.x, stateMachine.Player.transform.position.y + 1f, stateMachine.Player.transform.position.z);
            Vector3 dir = stateMachine.Player.MainCameraTransform.forward;
            stateMachine.Player.tornadoSkillObject.SetActive(true);

            //skillMachine.Player.skillObject.transform.forward = skillMachine.Player.dir;
            //skillMachine.Player.skillObject.transform.rotation = Quaternion.Euler(skillMachine.Player.skillObject.transform.rotation.x, 0f, skillMachine.Player.skillObject.transform.rotation.z);
            //skillMachine.Player.skillObject.transform.eulerAngles = new Vector3(skillMachine.Player.skillObject.transform.rotation.x, 0f, skillMachine.Player.skillObject.transform.rotation.z);
            //skillMachine.Player.skillObject.GetComponent<Rigidbody>().AddForce(skillMachine.Player.skillObject.transform.forward * 30f, ForceMode.Impulse);

            float time = 0;
            while (time < 1f)
            {
                time += Time.deltaTime;
                stateMachine.Player.tornadoSkillObject.transform.position += dir * 30f * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            stateMachine.Player.tornadoSkillObject.GetComponent<TornadoEvent>().Tornado();
            stateMachine.Player.Rigidbody.useGravity = true;
            stateMachine.ChangeState(stateMachine.FallingState);
        }

        public override void OnAnimationTransitionEvent()
        {
            stateMachine.Player.StopCor(TornadoCor());
            stateMachine.Player.StartCor(TornadoCor());
        }


    }
}
