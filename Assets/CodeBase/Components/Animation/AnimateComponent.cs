using UnityEngine;

namespace CodeBase.Components.Animation
{
    public sealed class AnimateComponent : MonoBehaviour
    {
        [SerializeField] private Animation[] _animations;
        
        public void PlayAnimation(Collider _)
        {
            foreach (var animation in _animations)
                animation.Play();
        }

        private void OnValidate()
        {
            for (var i = 0; i < _animations.Length; i++)
            {
                var animation = _animations[i];
                animation.OnValidate();
            }
        }

        private void Update()
        {
            foreach (var animation in _animations)
            {
                animation.OnUpdate(Time.deltaTime);
            }
        }
    }
}