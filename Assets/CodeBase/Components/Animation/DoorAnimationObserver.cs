using UnityEngine;

namespace CodeBase.Components.Animation
{
    public class DoorAnimationObserver : MonoBehaviour
    {
        [SerializeField] ColliderTriggerComponent _collider;
        [SerializeField] AnimateComponent _animate;

        private void OnEnable()
        {
            _collider.OnTriggerEntered += _animate.PlayAnimation;
        }

        private void OnDisable()
        {
            _collider.OnTriggerEntered -= _animate.PlayAnimation;
        }
    }
}