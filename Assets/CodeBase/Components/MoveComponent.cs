using UnityEngine;

namespace CodeBase.Components
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class MoveComponent : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _speed = 5f;

        private bool _canMove = false;
        private Vector3 _movementDirection;
    
        public void OnMove(Vector2 vector, bool isDragging)
        {
            _canMove = isDragging;

            _movementDirection = new Vector3(vector.x, 0, vector.y).normalized;
        }

        private void FixedUpdate()
        {
            if (_canMove)
            {
                Vector3 worldMovement = transform.TransformDirection(_movementDirection);

                _rigidbody.MovePosition(_rigidbody.position + worldMovement * _speed * Time.fixedDeltaTime);
            }
        }
    }
}