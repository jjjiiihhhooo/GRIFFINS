using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace genshin
{
    public class Skill
    {
        protected SkillMachine skillMachine;

        public float damage;

        public Skill(SkillMachine SkillMachine)
        {
            skillMachine = SkillMachine;
        }

        public virtual void Action()
        {
            skillMachine.Player.isSkill = true;
        }

        public virtual void Exit()
        {
            skillMachine.Player.isSkill = false;
        }

        protected void StartAnimation(int animationHash, int check = 0)
        {
            if (check == 1)
            {
                skillMachine.Player.Animator.SetTrigger(animationHash);
                return;
            }

            skillMachine.Player.Animator.SetBool(animationHash, true);
        }

        protected void StopAnimation(int animationHash, int check = 0)
        {
            if (check == 1)
            {
                return;
            }
            skillMachine.Player.Animator.SetBool(animationHash, false);
        }
    }
}
