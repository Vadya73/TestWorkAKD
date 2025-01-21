using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CodeBase.InputSystem
{
    public class Joystick : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        [SerializeField] private RectTransform handle;
        [SerializeField] private RectTransform background;
        [SerializeField] private float handleRange = 100f;

        public event Action<Vector2, bool> OnDraggable;
        private Vector2 InputDirection { get; set; }
        private bool isDragging = false;

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 position = RectTransformUtility.WorldToScreenPoint(null, background.position);
            Vector2 direction = (eventData.position - position) / handleRange;

            InputDirection = direction.magnitude > 1 ? direction.normalized : direction;

            handle.anchoredPosition = InputDirection * handleRange;
            isDragging = true;
            OnDraggable?.Invoke(InputDirection, isDragging);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            InputDirection = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
            isDragging = false;
            OnDraggable?.Invoke(InputDirection, isDragging);
        }
    }
}