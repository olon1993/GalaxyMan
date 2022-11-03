using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public abstract class CollisionInteraction2d : MonoBehaviour, IInteractive<Collider2D>
    {

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        [TagSelector] public string[] SelectedTags;
        private Collider2D _collider;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        protected virtual void Awake()
        {
            _collider = GetComponent<Collider2D>();
            if (_collider == null)
            {
                Debug.LogError("Collider not found on " + name);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            foreach (string tag in SelectedTags)
            {
                if (collision.CompareTag(tag))
                {
                    Interact(collision);
                }
            }
        }

        public abstract void Interact(Collider2D collider);
    }
}
