using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rehcub.ModularizedInput
{
    public class VectorModule : InputModule
    {
        [SerializeField] private bool _normalizeInput;

        public event Action<Vector2> OnDirectionChange;
        public Vector2 Direction { get => _currentDirection; }
        private Vector2 _currentDirection;
        private Vector2 _lastDirection;


        protected override void OnTriggered(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                _currentDirection = Vector2.zero;
                if (_currentDirection != _lastDirection)
                    OnDirectionChange?.Invoke(_currentDirection);
                return;
            }

            _currentDirection = context.ReadValue<Vector2>();
            if(_normalizeInput)
                _currentDirection.Normalize();

            if(_currentDirection != _lastDirection)
                OnDirectionChange?.Invoke(_currentDirection);

            _lastDirection = _currentDirection;
        }
    }
}