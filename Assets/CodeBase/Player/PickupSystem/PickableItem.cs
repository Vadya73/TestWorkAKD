using UnityEngine;

namespace CodeBase.Player.PickupSystem
{
    public class InteractableObject : MonoBehaviour, IInteractable
    {
        public GameObject GetObject()
        {
            return this.gameObject;
        }
    }
}