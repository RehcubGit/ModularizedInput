using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rehcub.ModularizedInput
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private string _usedActionMap = "GamePlay";

        private Dictionary<string, Action<InputAction.CallbackContext>> _inputActions;

        private void OnValidate()
        {
            _playerInput = GetComponent<PlayerInput>();
        }

        private void Awake()
        {
            _inputActions = new Dictionary<string, Action<InputAction.CallbackContext>>();
            if(_playerInput != null)
                InitializeInput(_playerInput);
        }

        public void InitializeInput(PlayerInput playerInput)
        {
            _playerInput = playerInput;

            _playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
            InputActionMap gameplayActionMap = _playerInput.actions.FindActionMap(_usedActionMap);
            RegisterActions(gameplayActionMap);
            gameplayActionMap.Enable();
            _playerInput.onActionTriggered += OnActionTriggered;
        }


        public void AddToAction(InputAction inputAction, Action<InputAction.CallbackContext> action) => AddToAction(inputAction.name, action);
        public void AddToAction(string name, Action<InputAction.CallbackContext> action)
        {
            if (_inputActions.ContainsKey(name) == false)
            {
                Debug.Log($"Action {name} does not exist!");
                return;
            }

            void EventHandler(InputAction.CallbackContext context)
            {
                try
                {
                    action(context);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Action {name} got removed, because an error occurred.\n{e}");
                    _inputActions[name] -= EventHandler;
                }
            }

            _inputActions[name] += EventHandler;
        }

        public Action OnButtonDown(InputAction inputAction, Action action) => OnButtonDown(inputAction.name, action);
        public Action OnButtonDown(string name, Action action)
        {
            if (_inputActions.ContainsKey(name) == false)
            {
                Debug.Log($"Action {name} does not exist!");
                return null;
            }

            void EventHandler(InputAction.CallbackContext context)
            {
                try
                {
                    if(context.GetButtonDown())
                        action();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Action {name} got removed, because an error occurred.\n{e}");
                    removeEventHandler();
                }
            }

            _inputActions[name] += EventHandler;

            void removeEventHandler() => _inputActions[name] -= EventHandler;
            return removeEventHandler;
        }

        private void RegisterActions(InputActionMap actionMap)
        {
            foreach (InputAction action in actionMap.actions)
            {
                _inputActions.Add(action.name, (ctx) => { });
            }
        }

        private void OnActionTriggered(InputAction.CallbackContext context)
        {
            if (_inputActions.ContainsKey(context.action.name) == false)
                return;

            _inputActions[context.action.name].Invoke(context);
        }

        private void OnDestroy()
        {
            _playerInput.onActionTriggered -= OnActionTriggered;
        }
    }
}