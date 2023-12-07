using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace genshin
{
    public interface IState
    {
        public void Enter();
        public void Exit();
        public void HandleInput();
        public void Update();
        public void PhysicsUpdate();
        public void OnTriggerEnter(Collider collider);
        public void OnTriggerExit(Collider collider);
        public void OnTriggerStay(Collider collider);
        public void OnAnimationEnterEvent();
        public void OnAnimationExitEvent();
        public void OnAnimationTransitionEvent();
    }
}