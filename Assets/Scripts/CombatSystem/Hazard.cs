using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class Hazard : MonoBehaviour
    {
        [SerializeField] protected bool _showDebugLog = false;
        [SerializeField] protected IDamage _damage;
        [SerializeField] protected string _ignoreTag= "Untagged";

        private void Awake()
        {
            _damage = GetComponent<IDamage>();
            if (_damage == null)
            {
                Debug.LogError("Damage not found on " + gameObject.name);
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            if (_showDebugLog) {
                Debug.Log("Hazard hit :" + col.gameObject.name);
			}
            if (col.tag == null) {
                return;
            }
            if (col.CompareTag(_ignoreTag)) {
                return;
            }

            IHealth otherHealth = col.GetComponent<IHealth>();
            if (otherHealth != null)
            {
                otherHealth.TakeDamage(_damage);
            }

            IRecoil recoil = col.GetComponent<IRecoil>();
            if (recoil != null)
            {
                Debug.Log("Recoil");
                float damageDirection = transform.position.x < col.transform.position.x ? 1 : -1;
                recoil.ApplyDamageForce(_damage.KnockbackForce, damageDirection);
            }
        }
    }
}

