using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TheFrozenBanana
{
	public class Health : MonoBehaviour, IHealth
	{
		[SerializeField] protected bool _showDebugLog = false;

		//**************************************************\\
		//********************* Fields *********************\\
		//**************************************************\\

		[SerializeField] private int _maxHealth;
		[SerializeField] protected int _currentHealth;
		[SerializeField] private float _timeToDie;
		[SerializeField] private Slider hpSlider;
		[SerializeField] private bool _destroyBounds = false;
		protected bool _isDead;
		private bool _isHurt;

		//**************************************************\\
		//******************** Methods *********************\\
		//**************************************************\\

		protected virtual void Awake() {
			if (CurrentHealth == 0) {
				CurrentHealth = MaxHealth;
			}
			if (hpSlider != null) {
				hpSlider.maxValue = _maxHealth;
				hpSlider.minValue = 0;
				hpSlider.value = _currentHealth;
			}
		}

		public virtual void TakeDamage(IDamage damage) {
			if (_isDead) {
				if (_showDebugLog) {
					Debug.Log(gameObject.name + " is already dead.");
				}
				return;
			}
			if (_showDebugLog) {
				Debug.Log(gameObject.name + " takes damage");
			}
			_currentHealth -= damage.DamageAmount;
			_isHurt = true;
			KillCheck();
		}

		protected virtual void KillCheck() {
			if (_currentHealth <= 0) {
				Die();
				UpdateSlider(0);
			} else {
				UpdateSlider(_currentHealth);
				StartCoroutine(FadeHurt());
			}
		}

		protected virtual void UpdateSlider(float val) {
			if (hpSlider != null) {
				hpSlider.value = val;
			}
		}

		protected virtual IEnumerator FadeHurt() {
			yield return new WaitForEndOfFrame();
			_isHurt = false;
		}

		public void AddHealth(int hp) {
			_currentHealth += hp;
			if (_currentHealth > _maxHealth) {
				_currentHealth = _maxHealth;
			}
		}

		protected virtual void Die() {
			_isDead = true;

			if (_showDebugLog) {
				Debug.Log(gameObject.name + " has died!");
			}
			if (_destroyBounds) {
				gameObject.GetComponent<Collider2D>().enabled = false;
			}
			StartCoroutine(DelayDeath());
		}

		private IEnumerator DelayDeath() {
			yield return new WaitForSeconds(_timeToDie);
			gameObject.SetActive(false);
		}

		//**************************************************\\
		//******************* Properties *******************\\
		//**************************************************\\

		public bool IsDead { get { return _isDead; } }

		public bool IsHurt {
			get { return _isHurt; }
			set { _isHurt = value; }
		}

		public int MaxHealth {
			get { return _maxHealth; }
			set { _maxHealth = value; }
		}

		public int CurrentHealth {
			get { return _currentHealth; }
			set { _currentHealth = value; }
		}

		public float TimeToDie {
			get { return _timeToDie; }
			set { _timeToDie = value; }
		}
	}
}