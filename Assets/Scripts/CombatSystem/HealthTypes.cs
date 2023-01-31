using System.Collections;
using UnityEngine;

namespace TheFrozenBanana
{
	public class HealthTypes : Health, IHealthTypes
	{
		//**************************************************\\
		//********************* Fields *********************\\
		//**************************************************\\

		[SerializeField] private IDamage.DamageType[] _resistanceTypeDefinition;
		[SerializeField] private IDamage.DamageType[] _vulnerableTypeDefinition;
		[SerializeField] private float _timeInvulnerable;
		[SerializeField] private float _resistFactor = 0.5f;
		[SerializeField] private float _vulnerableFactor = 1.5f;
		[SerializeField] private GameObject _spawnThisOnDeath;

		//**************************************************\\
		//******************** Methods *********************\\
		//**************************************************\\

		protected override void Awake() {
			base.Awake();
		}

		protected void OnEnable() {
			if (IsHurt) {
				StartCoroutine(FadeHurt());
			}
		}

		public override void TakeDamage(IDamage dmgDefinition) {
			if (!healthActive) {
				return;
			}
			if (IsHurt) {
				return;
			}
			if (_isDead) {
				if (_showDebugLog) {
					Debug.Log(gameObject.name + " is already dead.");
				}
				return;
			}
			if (_showDebugLog) {
				Debug.Log(gameObject.name + " takes damage");
			}

			float effectiveDamage = dmgDefinition.DamageAmount;
			bool resist = false;
			bool vulnerable = false;

			for (int i = 0; i < _resistanceTypeDefinition.Length; i++) {
				if ((int)_resistanceTypeDefinition[i] == (int)dmgDefinition.DamageTypeDefinition) {
					effectiveDamage *= _resistFactor;
					resist = true;
					break;
				}
			}

			if (!resist) {
				for (int i = 0; i < _vulnerableTypeDefinition.Length; i++) {
					if ((int)_vulnerableTypeDefinition[i] == (int)dmgDefinition.DamageTypeDefinition) {
						effectiveDamage *= _vulnerableFactor;
						vulnerable = true;
						break;
					}
				}
			}

			effectiveDamage = Mathf.RoundToInt(effectiveDamage);
			CurrentHealth -= (int)effectiveDamage;

			if (_showDebugLog) {
				Debug.Log(gameObject.name + ": Current Health: " + _currentHealth);
			}
			if (_showDebugLog) {
				if (vulnerable) {
					Debug.Log(gameObject.name + " vulnerable hit! (" + effectiveDamage + ")");
				} else if (resist) {
					Debug.Log(gameObject.name + " resistant hit! (" + effectiveDamage + ")");
				} else {
					Debug.Log(gameObject.name + " regular hit! (" + effectiveDamage + ")");
				}
			}

			KillCheck();
		}

		protected override IEnumerator FadeHurt() {
			IsHurt = true;
			yield return new WaitForSeconds(_timeInvulnerable);
			IsHurt = false;
		}


		protected override IEnumerator DelayDeath() {
			if (_spawnThisOnDeath != null) {
				Instantiate(_spawnThisOnDeath, transform.position, Quaternion.identity, null);
			}
			yield return new WaitForSeconds(_timeToDie);
			IDropLoot idl = gameObject.GetComponent<IDropLoot>();
			if (idl != null) {
				idl.DropRandomLoot();
			}

			gameObject.SetActive(false);
		}


		//**************************************************\\
		//******************* Properties *******************\\
		//**************************************************\\

		public IDamage.DamageType[] ResistanceTypeDefinition {
			get { return _resistanceTypeDefinition; }
		}

		public IDamage.DamageType[] VulnerableTypeDefinition {
			get { return _vulnerableTypeDefinition; }
		}

		public float TimeInvulnerable {
			get { return _timeInvulnerable; }
		}
	}
}