using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace genshin
{
    public abstract class StateMachine
    {
        protected IState currentState;
        protected IState previousState;

        public void ChangeState(IState newState)
        {
            
            currentState?.Exit();
            previousState = currentState;
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

        public System.Type GetPreviousState()
        {
            return previousState.GetType();
        }

        public void OnTriggerEnter(Collider collider)
        {
            currentState?.OnTriggerEnter(collider);
        }

        public void OnTriggerExit(Collider collider)
        {
            currentState?.OnTriggerExit(collider);
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



