using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana
{

	public class FireGunAction : BossAction
	{
		[SerializeField] private GameObject ProjectilePrefab;
		[SerializeField] private int ShotsPerAction;
		[SerializeField] private float AngleChangePerShot;

		protected override IEnumerator CarryOutSpecificAction(int phase)
		{
			float t = 0f;
			int shot = 1;
			float horizontalSign = Mathf.Sign(BossManagerScript.Target.position.x - transform.position.x);
			float verticalSign = Mathf.Sign(BossManagerScript.Target.position.y - (2f) - transform.position.y);
			Vector2 direction = new Vector2(horizontalSign, verticalSign);
			float startAngle = -30f;
			if (direction.y > 0)
			{
				startAngle = 15f;
			}
			float angle = startAngle;
			float deltaTime = ActionTime[phase] / ShotsPerAction;
			while (t < ActionTime[phase])
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
			GameObject tmp = Instantiate(ProjectilePrefab, BossManagerScript.Weapon.transform.position, Quaternion.identity, null) as GameObject;
			Vector3 fireDirection = tmp.transform.position + rotation * Vector3.right * 20;
			tmp.GetComponent<IProjectile>().Setup(tmp.transform.position, fireDirection, "Enemy");
		}
	}

}
