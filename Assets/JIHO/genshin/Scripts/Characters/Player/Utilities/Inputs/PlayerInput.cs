using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

}

