using CodeBase.Components;
using CodeBase.InputSystem;
using UnityEngine;

namespace CodeBase.Player
{
    public sealed class PlayerMoveObserver : MonoBehaviour
    {
        [Header("Player Movement")]
        [SerializeField] private Joystick _moveJoystick;
        [SerializeField] private MoveComponent _moveComponent;
        [Header("Player Rotate")]
        [SerializeField] private Joystick _rotateJoystick;
        [SerializeField] private RotateComponent _rotateComponent;

        private void OnEnable()
        {
            _moveJoystick.OnDraggable += _moveComponent.OnMove;
            _rotateJoystick.OnDraggable += _rotateComponent.OnRotate;
        }

        private void OnDisable()
        {
            _moveJoystick.OnDraggable -= _moveComponent.OnMove;
            _rotateJoystick.OnDraggable -= _rotateComponent.OnRotate;
        }
    }
}