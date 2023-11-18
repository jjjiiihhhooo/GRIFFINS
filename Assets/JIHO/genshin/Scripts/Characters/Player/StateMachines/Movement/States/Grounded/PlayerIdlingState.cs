using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace genshin
{
    public class PlayerIdlingState : PlayerGroundedState
    {
        public PlayerIdlingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            speedModifier = 0f;

            ResetVelocity();
        }

        public override void Update()
        {
            base.Update();

            if(movementInput == Vector2.zero)
            {
                return;
            }

            OnMove();
        }

        
        #endregion
    }

}
