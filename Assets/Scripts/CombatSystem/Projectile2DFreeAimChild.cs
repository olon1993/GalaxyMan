using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{

	public class Projectile2DFreeAimChild : MonoBehaviour
	{

		[SerializeField] private IProjectile projectileParent;
		private Collider2D myCollider;

		private void Awake()
		{
			projectileParent = gameObject.GetComponentInParent<IProjectile>();
			myCollider = gameObject.GetComponent<Collider2D>();
			if (myCollider == null)
			{
				Debug.LogError(gameObject.GetComponentInParent<GameObject>().name + " has no proper collider setup! Check child in Prefab!");
			}
		}

		private void OnTriggerEnter2D(Collider2D col)
		{
			Debug.Log("This is an old script. You should use the new one.");
			//projectileParent.TriggerCollision(col);
		}
	}
}