using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Player.PickupSystem
{
    public sealed class ObjectPickupManager : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private Button _pickupButton;
        [SerializeField] private TMP_Text _buttonText; // Ссылка на текст в кнопке
        [SerializeField] private Button _carHoldButton;
        [SerializeField] private Transform _holdPosition; // Позиция удержания предмета
        [SerializeField] private Collider _truckZoneCollider;         // Коллайдер зоны для фургона (место для хранения)
        [SerializeField] private float _smoothMoveSpeed = 5f; // Скорость перемещения объекта
        [SerializeField] private float _rotationSpeed = 10f; // Скорость вращения
        [SerializeField] private Transform[] _storagePositions;       // Места хранения внутри кузова

        private IInteractable _currentInteractable = null;
        private Rigidbody _heldObjectRigidbody; // Rigidbody удерживаемого объекта

        private void Start()
        {
            if (_mainCamera == null)
                _mainCamera = Camera.main;

            if (_pickupButton == null)
                Debug.LogError("Pickup button is not assigned!");
            
            if (_buttonText == null)
                _buttonText = _pickupButton.GetComponentInChildren<TMP_Text>();

            _pickupButton.gameObject.SetActive(false);
            _carHoldButton.gameObject.SetActive(false);
            _pickupButton.onClick.AddListener(OnPickupButtonClicked);
            _carHoldButton.onClick.AddListener(PlaceInTruck);
        }

        private void Update()
        {
            // Проверка касания экрана для телефона, на компьютере не работает, но по тестовому нужно всё делать под телефон
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
            
                Ray touchRay = _mainCamera.ScreenPointToRay(touch.position);

                RaycastHit Touchhit;
                if (Physics.Raycast(touchRay, out Touchhit, 100f))
                {
                    if (!Touchhit.collider.TryGetComponent(out IInteractable interactable))
                        return;
                    
                    if (_currentInteractable == null)
                    {
                        _currentInteractable = interactable;
                        PickupObject();
                    }
                }
            }

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Pickup");
                var touch = Input.mousePosition;
            
                Ray touchRay = _mainCamera.ScreenPointToRay(touch);

                RaycastHit Touchhit;
                if (Physics.Raycast(touchRay, out Touchhit, 50f))
                {
                    Debug.Log("Pickup2" + Touchhit.collider.name);
                    if (!Touchhit.collider.TryGetComponent(out IInteractable interactable))
                        return;
                    
                    Debug.Log("Pickup3");
                    if (_currentInteractable == null)
                    {
                        Debug.Log("Pickup4");
                        _currentInteractable = interactable;
                        PickupObject();
                    }
                }
            }
#endif
            
            Ray ray = _mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                if (hit.collider == _truckZoneCollider && _currentInteractable != null)
                {
                    _carHoldButton.gameObject.SetActive(true);
                }
                
                if (hit.collider.TryGetComponent(out IInteractable interactable))
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

        private void FixedUpdate()
        {
            if (_heldObjectRigidbody != null)
            {
                _heldObjectRigidbody.velocity = (_holdPosition.position - _heldObjectRigidbody.position) * _smoothMoveSpeed;

                Quaternion targetRotation = _holdPosition.rotation;
                _heldObjectRigidbody.MoveRotation(Quaternion.Slerp(_heldObjectRigidbody.rotation, targetRotation, Time.fixedDeltaTime * _rotationSpeed));
            }
        }

        private void OnPickupButtonClicked()
        {
            if (_heldObjectRigidbody != null)
                DropObject();
            else if (_currentInteractable != null)
                PickupObject();
        }

        private void PickupObject()
        {
            _heldObjectRigidbody = _currentInteractable.GetObject().GetComponent<Rigidbody>();
            if (_heldObjectRigidbody != null)
            {
                _pickupButton.gameObject.SetActive(true);

                _heldObjectRigidbody.isKinematic = false;
                _heldObjectRigidbody.useGravity = false;
                
                _buttonText.text = "Drop";
            }
        }

        private void DropObject()
        {
            _heldObjectRigidbody.useGravity = true;
            _heldObjectRigidbody.isKinematic = false;
            
            _buttonText.text = "Pick Up";

            _heldObjectRigidbody = null;
            _currentInteractable = null;
        }

        private void HidePickupButtonIfNotHolding()
        {
            if (_heldObjectRigidbody == null)
            {
                _pickupButton.gameObject.SetActive(false);
            }
        }
        
        private void PlaceInTruck()
        {
            if (_storagePositions.Length > 0)
            {
                foreach (var storagePosition in _storagePositions)
                {
                    if (storagePosition.childCount == 0)
                    {
                        _heldObjectRigidbody.transform.position = storagePosition.position;
                        _heldObjectRigidbody.transform.rotation = storagePosition.rotation;

                        _heldObjectRigidbody.isKinematic = false;
                        _heldObjectRigidbody.useGravity = true;

                        _heldObjectRigidbody.transform.SetParent(null);
                        _heldObjectRigidbody = null;

                        _carHoldButton.gameObject.SetActive(false);
                        _currentInteractable = null;

                        break;
                    }
                }
            }
        }
    }
}
