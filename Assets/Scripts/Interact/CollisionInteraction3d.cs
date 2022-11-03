using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public abstract class CollisionInteraction3d : MonoBehaviour, IInteractive<Collider>
    {

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        [TagSelector] public string[] SelectedTags;
        private Collider _collider;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        protected virtual void Awake()
        {
            _collider = GetComponent<Collider>();
            if(_collider == null)
            {
                Debug.LogError("Collider not found on " + name);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            foreach(string tag in SelectedTags)
            {
                if (other.CompareTag(tag))
                {
                    Interact(other);
                }
            }
        }

        public abstract void Interact(Collider collider);
    }
}
