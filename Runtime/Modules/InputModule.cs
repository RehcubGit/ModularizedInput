using UnityEngine;
using UnityEngine.InputSystem;

namespace Rehcub.ModularizedInput
{
    [RequireComponent(typeof(InputHandler))]
    public abstract class InputModule : MonoBehaviour
    {
        [SerializeField] private InputHandler _inputHandler;
        [SerializeField] private string _inputAction;
        [SerializeField] private bool _activateOnAwake;

        protected bool _activ;

        protected virtual void OnValidate()
        {
            _inputHandler = GetComponent<InputHandler>();
        }

        protected virtual void Start()
        {
            if (_activateOnAwake)
                Activate();
            _inputHandler.AddToAction(_inputAction, OnInputTriggered);
        }

        public virtual void Activate() => _activ = true;

        private void OnInputTriggered(InputAction.CallbackContext context)
        {
            if (_activ == false)
                return;

            OnTriggered(context);
        }

        protected abstract void OnTriggered(InputAction.CallbackContext context);
    }
}