using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{

	public class FireGunAction : BossAction
	{
		[SerializeField] private GameObject ProjectilePrefab;
		[SerializeField] private int ShotsPerAction;
		[SerializeField] private float AngleChangePerShot;

		protected override IEnumerator CarryOutSpecificAction()
		{
			float t = 0f;
			int shot = 1;
			float horizontalSign = Mathf.Sign(BossManager.Target.position.x - transform.position.x);
			float verticalSign = Mathf.Sign(BossManager.Target.position.y - (2f) - transform.position.y);
			Vector2 direction = new Vector2(horizontalSign, verticalSign);
			float startAngle = -30f;
			if (direction.y > 0)
			{
				startAngle = 15f;
			}
			float angle = startAngle;
			float deltaTime = TotalActionTime / ShotsPerAction;
			while (t < TotalActionTime)
			{
				yield return new WaitForSeconds(deltaTime);
				t += deltaTime;
				CreateProjectile(direction, angle);
				shot++;
				angle += AngleChangePerShot;
			}
			_actionBusy = false;
		}

		private void CreateProjectile(Vector2 direction, float angle)
		{
			Quaternion rotation = Quaternion.Euler(0, 0, angle);
			if (direction.x < 0)
			{
				rotation = Quaternion.Euler(0, 0, -angle);
				rotation *= Quaternion.Euler(0, 0, 180);
			}
			GameObject tmp = Instantiate(ProjectilePrefab, BossManager.Weapon.transform.position, Quaternion.identity, null) as GameObject;
			Vector3 fireDirection = tmp.transform.position + rotation * Vector3.right * 20;
			tmp.GetComponent<Projectile>().Setup(tmp.transform.position, fireDirection, rotation, "Enemy");
		}
	}

}
