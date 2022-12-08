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
        [SerializeField] protected bool _CalculateRecoilFromCenter = false;

        private void Awake()
        {
            _damage = GetComponent<IDamage>();
            if (_damage == null)
            {
                Debug.LogError("Damage not found on " + gameObject.name);
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D col) {
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
            if (otherHealth != null) {
                otherHealth.TakeDamage(_damage);
            }

            IRecoil recoil = col.GetComponent<IRecoil>();
            if (recoil != null) {
                if (_showDebugLog) {
                    Debug.Log("Recoil");
                }
                Vector2 closest = col.ClosestPoint(gameObject.transform.position);
                Vector3 src = new Vector3(closest.x, closest.y, 0);
                if (_CalculateRecoilFromCenter) {
                    src = gameObject.transform.position;
				}
                recoil.ApplyRecoil(_damage.KnockbackForce, src);
            }
        }
    }
}

