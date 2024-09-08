using System;
using System.Diagnostics;
using UnityEngine.InputSystem;

namespace Rehcub.ModularizedInput
{
    public class ButtonModule : InputModule
    {
        public event Action OnButtonDown;
        public event Action OnButtonUp;

        public bool IsPressed { get => _buttonPressed; }
        private bool _buttonPressed;


        protected override void OnTriggered(InputAction.CallbackContext context)
        {
            if (context.GetButtonDown())
            {
                _buttonPressed = true;
                OnButtonDown?.Invoke();
            }

            if (context.GetButtonUp())
            {
                _buttonPressed = false;
                OnButtonUp?.Invoke();
            }
        }
    }
}