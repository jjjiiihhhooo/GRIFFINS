using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace genshin
{
    public class SkillMachine
    {
        public Player Player { get; }
        public TornadoSkill Tornado { get; }
        public BreezeSkill Breeze { get; }

        public SkillMachine(Player player)
        {
            Player = player;
            Tornado = new TornadoSkill(this);
            Breeze = new BreezeSkill(this);
        }


    }
}
