using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace genshin
{
    public abstract class StateMachine
    {
        protected IState currentState;

        public void ChangeState(IState newState)
        {
            
            currentState?.Exit();

            currentState = newState;

            Debug.Log(currentState?.GetType().Name);
            currentState.Enter();
        }

        public System.Type GetCurrentStateType()
        {
            return currentState.GetType();
        }

        public void HandleInput()
        {
            currentState?.HandleInput();
        }

        public void Update()
        {
            currentState?.Update();
        }

        public void PhysicsUpdate()
        {
            currentState?.PhysicsUpdate();
        }

        public void OnTriggerEnter(Collider collider)
        {
            currentState?.OnTriggerEnter(collider);
        }

        public void OnTriggerExit(Collider collider)
        {
            currentState?.OnTriggerExit(collider);
        }

        public void OnTriggerStay(Collider collider)
        {
            currentState?.OnTriggerStay(collider);
        }

        public void OnAnimationEnterEvent()
        {
            currentState?.OnAnimationEnterEvent();
        }

        public void OnAnimationExitEvent()
        {
            currentState?.OnAnimationExitEvent();
        }

        public void OnAnimationTransitionEvent()
        {
            currentState?.OnAnimationTransitionEvent();
        }
    }
}



