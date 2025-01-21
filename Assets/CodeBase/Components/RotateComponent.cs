using UnityEngine;

namespace CodeBase.Components
{
    public sealed class RotateComponent : MonoBehaviour
    {
        [SerializeField] private Transform _playerBody;
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private float _rotateSpeed = 0.5f;
        [SerializeField] private float _verticalClamp = 80f;

        private bool _canRotate = false;
        private Vector2 _rotationInput;
        private float _verticalRotation = 0f;

        public void OnRotate(Vector2 input, bool isDragging)
        {
            _canRotate = isDragging;
            _rotationInput = input;
        }

        private void Update()
        {
            if (_canRotate)
            {
                RotatePlayer();
            }
        }

        private void RotatePlayer()
        {
            _playerBody.Rotate(Vector3.up, _rotationInput.x * _rotateSpeed);

            _verticalRotation -= _rotationInput.y * _rotateSpeed;
            _verticalRotation = Mathf.Clamp(_verticalRotation, -_verticalClamp, _verticalClamp);

            _playerCamera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);
        }
    }
}