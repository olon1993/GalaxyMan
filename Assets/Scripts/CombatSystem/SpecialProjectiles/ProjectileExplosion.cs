using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class ProjectileExplosion : MonoBehaviour
    {
		[SerializeField] private int _projectilesToSpawn;
		[SerializeField] private GameObject _projectileGameObject;
		[SerializeField] private float _projectilePossibleAngle;

		private void Awake() {
			float deltaAngle = 0;
			float startAngle = 90;
			if (_projectilesToSpawn > 1) {
				deltaAngle = _projectilePossibleAngle / (float)((float)_projectilesToSpawn - 1f);
				startAngle = 90 + _projectilePossibleAngle / 2;
			}
			float currentAngle = startAngle;
			Vector3 projectileTargetLocation = new Vector3();
			for (int i = 0; i < _projectilesToSpawn; i++) {
				
				float y = 5f * Mathf.Pow(Mathf.Sin((currentAngle * Mathf.PI) / 180),2);
				float x = 5f - y;
				if (currentAngle > 90f) {
					x = -x;
				}
				projectileTargetLocation = transform.position + new Vector3(x,y,0);

				GameObject projectile = Instantiate(_projectileGameObject,transform.position, Quaternion.identity, null) as GameObject;
				projectile.GetComponent<Projectile2d>().Setup(transform.position, projectileTargetLocation,"Enemy");
				currentAngle -= deltaAngle;
			}
			Destroy(this,0.5f);
		}
	}
}
