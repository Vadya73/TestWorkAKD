using UnityEngine;

namespace CodeBase.Player
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private RectTransform _touchZone;
        [SerializeField] private Transform _playerBody;
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private float _sensitivity = 0.5f;
        [SerializeField] private float _verticalClamp = 80f;
        private Vector2 previousTouchPosition;
        private float verticalRotation = 0f;

        private bool IsTouchInsideZone(Vector2 touchPosition)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(_touchZone, touchPosition);
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                Debug.Log(Input.touchCount);
                Touch touch = Input.GetTouch(0);

                if (IsTouchInsideZone(touch.position))
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        previousTouchPosition = touch.position;
                    }
                    else if (touch.phase == TouchPhase.Moved)
                    {
                        Vector2 delta = touch.position - previousTouchPosition;

                        _playerBody.Rotate(Vector3.up, delta.x * _sensitivity);

                        verticalRotation -= delta.y * _sensitivity;
                        verticalRotation = Mathf.Clamp(verticalRotation, -_verticalClamp, _verticalClamp);
                        _playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

                        previousTouchPosition = touch.position;
                    }
                }
            }
        }
    }
}
