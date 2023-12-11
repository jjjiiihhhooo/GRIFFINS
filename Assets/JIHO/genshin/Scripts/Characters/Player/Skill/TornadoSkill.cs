using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace genshin
{
    public class TornadoSkill : Skill
    {
        public TornadoSkill(SkillMachine SkillMachine) : base(SkillMachine)
        {
        }

        public override void Action()
        {
            base.Action();
            skillMachine.Player.Animator.SetTrigger("isTornado");
            skillMachine.Player.Rigidbody.velocity = Vector3.zero;
            skillMachine.Player.Rigidbody.useGravity = false;
            skillMachine.Player.pm.bounceCombine = PhysicMaterialCombine.Minimum;
            skillMachine.Player.transform.forward = skillMachine.Player.MainCameraTransform.forward;
        }

        public void TornadoStart()
        {
            skillMachine.Player.StopCor(TornadoCor());
            skillMachine.Player.StartCor(TornadoCor());

        }

        private IEnumerator TornadoCor()
        {
            skillMachine.Player.Rigidbody.useGravity = true;
            skillMachine.Player.tornadoSkillObject.transform.position = new Vector3(skillMachine.Player.transform.position.x, skillMachine.Player.transform.position.y + 1f, skillMachine.Player.transform.position.z);
            Vector3 dir = skillMachine.Player.MainCameraTransform.forward;

            skillMachine.Player.tornadoSkillObject.GetComponent<TornadoEvent>().TornadoEffect.SetActive(false);
            skillMachine.Player.tornadoSkillObject.SetActive(true);
            skillMachine.Player.movementStateMachine.ChangeState(skillMachine.Player.movementStateMachine.FallingState);

            //skillMachine.Player.skillObject.transform.forward = skillMachine.Player.dir;
            //skillMachine.Player.skillObject.transform.rotation = Quaternion.Euler(skillMachine.Player.skillObject.transform.rotation.x, 0f, skillMachine.Player.skillObject.transform.rotation.z);
            //skillMachine.Player.skillObject.transform.eulerAngles = new Vector3(skillMachine.Player.skillObject.transform.rotation.x, 0f, skillMachine.Player.skillObject.transform.rotation.z);
            //skillMachine.Player.skillObject.GetComponent<Rigidbody>().AddForce(skillMachine.Player.skillObject.transform.forward * 30f, ForceMode.Impulse);

            float time = 0;
            while(time < 1f)
            {
                time += Time.deltaTime;
                skillMachine.Player.tornadoSkillObject.transform.position += dir * 30f * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            skillMachine.Player.tornadoSkillObject.GetComponent<TornadoEvent>().Tornado();
            
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
