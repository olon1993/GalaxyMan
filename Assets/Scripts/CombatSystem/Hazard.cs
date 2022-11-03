using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class Hazard : MonoBehaviour
    {
        [SerializeField] private Damage _damage;

        private void Awake()
        {
            _damage = GetComponent<Damage>();
            if (_damage == null)
            {
                Debug.LogError("Damage not found on " + gameObject.name);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            IHealth otherHealth = other.GetComponent<IHealth>();
            if (otherHealth != null)
            {
                otherHealth.TakeDamage(_damage);
            }

            IRecoil recoil = other.GetComponent<IRecoil>();
            if (recoil != null)
            {
                float damageDirection = transform.position.x < other.transform.position.x ? 1 : -1;
                recoil.ApplyDamageForce(_damage.DamageForce, damageDirection);
            }
        }
    }
}

