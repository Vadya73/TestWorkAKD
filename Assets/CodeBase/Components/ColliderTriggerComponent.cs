using System;
using UnityEngine;

namespace CodeBase.Components
{
    public class ColliderTriggerComponent : MonoBehaviour
    {
        public event Action<Collider> OnTriggerEntered;
        private void OnTriggerEnter(Collider collideObject)
        {
            OnTriggerEntered?.Invoke(collideObject);
        }
    }
}
