using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace genshin
{
    public class PlayerInput : MonoBehaviour
    {
        public PlayerInputActions InputAction { get; private set; }
        public PlayerInputActions.PlayerActions PlayerActions { get; private set;}

        private void Awake()
        {
            InputAction = new PlayerInputActions();

            PlayerActions = InputAction.Player;
        }

        private void OnEnable()
        {
            InputAction.Enable();
        }

        private void OnDisable()
        {
            InputAction.Disable();
        }

        public void DisableActionFor(InputAction action, float seconds)
        {
            StartCoroutine(DisableAction(action, seconds));
        }

        private IEnumerator DisableAction(InputAction action, float seconds)
        {
            action.Disable();

            yield return new WaitForSeconds(seconds);

            action.Enable();
        }

    }
}

