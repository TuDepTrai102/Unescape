using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class Interactable : MonoBehaviour
    {
        public float radius = 0.6f;
        public string interactableText;

        protected virtual void Awake()
        {
            
        }

        protected virtual void Start()
        {

        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        public virtual void Interact(PlayerManager playerManager)
        {
            Debug.Log("You interacted with an object!");
        }
    }
}