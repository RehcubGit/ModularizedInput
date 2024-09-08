using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Rehcub.ModularizedInput
{
    public static class InputSystemExtensions
    {
        public static bool GetButton(this InputAction action) => action.ReadValue<float>() > 0;
        public static bool GetButtonDown(this InputAction action) => action.triggered && action.ReadValue<float>() > 0;
        public static bool GetButtonUp(this InputAction action) => action.triggered && action.ReadValue<float>() == 0;

        public static bool IsButton(this InputAction action) => action.type == InputActionType.Button;

        // https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.InputActionType.html#UnityEngine_InputSystem_InputActionType_PassThrough

        public static bool GetButtonDown(this InputAction.CallbackContext context)
        {
            switch (context.action.type)
            {
                case InputActionType.Button:
                    if (context.started)
                        return false;
                    return (context.control as ButtonControl).wasPressedThisFrame;
                case InputActionType.PassThrough:
                    if (context.control is ButtonControl button)
                        return button.wasPressedThisFrame;
                    break;
            }
            return context.started;
        }

        public static bool GetButton(this InputAction.CallbackContext context)
        {
            switch (context.action.type)
            {
                case InputActionType.Button:
                    return context.action.IsPressed();
                case InputActionType.PassThrough:
                    if (context.control is ButtonControl button)
                        return button.IsPressed();
                    break;
            }
            return context.performed;
        }

        public static bool GetButtonUp(this InputAction.CallbackContext context)
        {
            switch (context.action.type)
            {
                case InputActionType.Button:
                    return (context.control as ButtonControl).wasReleasedThisFrame;
                case InputActionType.PassThrough:
                    if (context.control is ButtonControl button)
                        return button.wasReleasedThisFrame;
                    break;
            }
            return context.canceled;
        }
    }
}