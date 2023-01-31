using System.Collections;
using System.Collections.Generic;
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

		protected DependencyManager _dependencyManager;
		private IInputManager _inputManager;

		[SerializeField] private int _maxHealth;
		[SerializeField] protected int _currentHealth;
		[SerializeField] protected float _timeToDie;
		[SerializeField] private Slider hpSlider;
		[SerializeField] private bool _destroyBounds = false;
		protected bool healthActive = true;
		protected bool _isDead;
		private bool _isHurt;

		//**************************************************\\
		//******************** Methods *********************\\
		//**************************************************\\

		protected virtual void Awake() {
			GetDependencies();
			if (CurrentHealth == 0) {
				CurrentHealth = MaxHealth;
			}
			if (hpSlider != null) {
				hpSlider.maxValue = _maxHealth;
				hpSlider.minValue = 0;
				hpSlider.value = _currentHealth;
			}
		}

		private void GetDependencies() {
			_dependencyManager = GetComponent<DependencyManager>();
			if (_dependencyManager == null) {
				Debug.LogError("DependencyManager not found on " + name);
			}
			try {
				_inputManager = (IInputManager)_dependencyManager.Registry[typeof(IInputManager)];
			} catch (KeyNotFoundException knfe) {
				Debug.Log("Object " + this.gameObject.name + " has HP, but no input manager found. Make sure this is by design. ");
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
			UpdateSlider(_currentHealth);
			if (_currentHealth > _maxHealth) {
				_currentHealth = _maxHealth;
			}
		}

		protected virtual void Die() {
			_isDead = true;
			if (_inputManager != null) {
				_inputManager.EndOverride();
				_inputManager.IsEnabled = false;
			}
			if (_showDebugLog) {
				Debug.Log(gameObject.name + " has died!");
			}
			if (_destroyBounds) {
				gameObject.GetComponent<Collider2D>().enabled = false;
			}
			StartCoroutine(DelayDeath());
		}

		protected virtual IEnumerator DelayDeath() {
			yield return new WaitForSeconds(_timeToDie);
			gameObject.SetActive(false);
		}


		public void ToggleHealthActive(bool option) {
			healthActive = option;
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