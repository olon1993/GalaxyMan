using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana
{
	public class Damage : MonoBehaviour, IDamage
	{
		//**************************************************\\
		//********************* Fields *********************\\
		//**************************************************\\

		[SerializeField] private IDamage.DamageType _damageTypeDefinition;
		[SerializeField] private float _stunTime;
		[SerializeField] private int _damageAmount;
		[SerializeField] private float _knockbackForce;

		//**************************************************\\
		//******************* Properties *******************\\
		//**************************************************\\

		public IDamage.DamageType DamageTypeDefinition {
			get { return _damageTypeDefinition; }
			set { _damageTypeDefinition = value; }
		}

		public int DamageAmount {
			get { return _damageAmount; }
			set { _damageAmount = value; }
		}
		public float KnockbackForce {
			get { return _knockbackForce; }
			set { _knockbackForce = value; }
		}
		public float StunTime {
			get { return _stunTime; }
			set { _stunTime = value; }
		}
	}
}