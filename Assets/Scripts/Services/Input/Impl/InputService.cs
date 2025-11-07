using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Services.Input.Impl
{
    public class InputService : IInputService, IDisposable
    {
        private readonly InputSystem_Actions _inputSystemActions = new();
        
        public Vector2 MoveDirection { get; private set; }
        public bool IsMoved { get; private set; }
        public Action OnCancelPressed { get; set; }

        public void Initialize()
        {
            _inputSystemActions.Player.Move.performed += MovePerformed;
            _inputSystemActions.Player.Move.canceled += MoveCancel;
            _inputSystemActions.Player.CancelFollow.performed += CancelFollowPerformed;
            
            _inputSystemActions.Enable();
        }
        
        private void CancelFollowPerformed(InputAction.CallbackContext _)
        {
            OnCancelPressed?.Invoke();
        }

        private void MovePerformed(InputAction.CallbackContext context)
        {
            if (!IsMoved)
            {
                IsMoved = true;
            }
            
            MoveDirection = context.ReadValue<Vector2>();
        }
        
        private void MoveCancel(InputAction.CallbackContext _)
        {
            if (IsMoved)
            {
                IsMoved = false;
            }
        }

        public void Dispose()
        {
            _inputSystemActions.Player.Move.performed -= MovePerformed;
            _inputSystemActions.Player.Move.canceled -= MoveCancel;
            _inputSystemActions.Player.CancelFollow.performed -= CancelFollowPerformed;
            
            _inputSystemActions.Disable();
            _inputSystemActions.Dispose();
        }
    }
}