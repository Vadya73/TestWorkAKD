using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Player.PickupSystem
{
    public sealed class ObjectPickupManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private Button _pickupButton;
        [SerializeField] private TMP_Text _buttonText;
        [SerializeField] private Button _carHoldButton;
        [SerializeField] private Transform _holdPosition;
        [SerializeField] private Collider _truckZoneCollider;
        [SerializeField] private Transform[] _storagePositions;

        [Header("Settings")]
        [SerializeField] private float _smoothMoveSpeed = 5f;
        [SerializeField] private float _rotationSpeed = 10f;

        private IInteractable _currentInteractable;
        private Rigidbody _heldObjectRigidbody;

        private void Awake()
        {
            InitializeReferences();
            BindButtonActions();
        }

        private void Update()
        {
            HandleInput();
            HandleRaycast();
        }

        private void FixedUpdate()
        {
            MoveHeldObject();
        }

        private void InitializeReferences()
        {
            _mainCamera ??= Camera.main;
            _buttonText ??= _pickupButton.GetComponentInChildren<TMP_Text>();
            _pickupButton.gameObject.SetActive(false);
            _carHoldButton.gameObject.SetActive(false);
        }

        private void BindButtonActions()
        {
            _pickupButton.onClick.AddListener(HandlePickupButtonClick);
            _carHoldButton.onClick.AddListener(PlaceInTruck);
        }

        private void HandleInput()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
                ProcessTouch(Input.mousePosition);
#else
            if (Input.touchCount > 0)
                ProcessTouch(Input.GetTouch(0).position);
#endif
        }

        private void ProcessTouch(Vector3 touchPosition)
        {
            Ray ray = _mainCamera.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f) && hit.collider.TryGetComponent(out IInteractable interactable))
            {
                if (_currentInteractable == null)
                {
                    _currentInteractable = interactable;
                    PickupObject();
                }
            }
        }

        private void HandleRaycast()
        {
            Ray ray = _mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                if (hit.collider == _truckZoneCollider && _currentInteractable != null)
                {
                    _carHoldButton.gameObject.SetActive(true);
                }
                else if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    _currentInteractable = interactable;
                    _pickupButton.gameObject.SetActive(true);
                }
                else
                {
                    HidePickupButtonIfNotHolding();
                }
            }
            else
            {
                HidePickupButtonIfNotHolding();
            }
        }

        private void MoveHeldObject()
        {
            if (_heldObjectRigidbody == null) return;

            _heldObjectRigidbody.velocity = (_holdPosition.position - _heldObjectRigidbody.position) * _smoothMoveSpeed;

            Quaternion targetRotation = _holdPosition.rotation;
            _heldObjectRigidbody.MoveRotation(Quaternion.Slerp(
                _heldObjectRigidbody.rotation, 
                targetRotation, 
                Time.fixedDeltaTime * _rotationSpeed
            ));
        }

        private void HandlePickupButtonClick()
        {
            if (_heldObjectRigidbody != null)
                DropObject();
            else if (_currentInteractable != null)
                PickupObject();
        }

        private void PickupObject()
        {
            GameObject interactableObject = _currentInteractable.GetObject();

            if (interactableObject.TryGetComponent(out Rigidbody objectRigidbody))
            {
                _heldObjectRigidbody = objectRigidbody;
                _heldObjectRigidbody.isKinematic = false;
                _heldObjectRigidbody.useGravity = false;

                _pickupButton.gameObject.SetActive(true);
                _buttonText.text = "Drop";
            }
        }

        private void DropObject()
        {
            if (_heldObjectRigidbody == null) return;

            _heldObjectRigidbody.useGravity = true;
            _heldObjectRigidbody.isKinematic = false;

            _buttonText.text = "Pick Up";

            _heldObjectRigidbody = null;
            _currentInteractable = null;
        }

        private void PlaceInTruck()
        {
            if (_heldObjectRigidbody == null) return;

            foreach (var storagePosition in _storagePositions)
            {
                if (storagePosition.childCount > 0) continue;

                _heldObjectRigidbody.transform.SetPositionAndRotation(
                    storagePosition.position,
                    storagePosition.rotation
                );

                _heldObjectRigidbody.isKinematic = false;
                _heldObjectRigidbody.useGravity = true;
                _heldObjectRigidbody.transform.SetParent(null);

                _carHoldButton.gameObject.SetActive(false);

                _heldObjectRigidbody = null;
                _currentInteractable = null;
                break;
            }
        }

        private void HidePickupButtonIfNotHolding()
        {
            if (_heldObjectRigidbody == null)
                _pickupButton.gameObject.SetActive(false);
        }
    }
}
