using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace genshin
{
    public class PlayerMovementStateMachine : StateMachine
    {
        public Player Player { get; }

        public PlayerIdlingState IdlingState { get; }
        public PlayerWalkingState WalkingState { get; }
        public PlayerRunningState RunningState { get; }
        public PlayerSprintState SprintState { get; }


        public PlayerMovementStateMachine(Player player)
        {
            Player = player;

            IdlingState = new PlayerIdlingState(this);
            
            WalkingState = new PlayerWalkingState(this);
            RunningState = new PlayerRunningState(this);
            SprintState = new PlayerSprintState(this);
        }

    }
}


