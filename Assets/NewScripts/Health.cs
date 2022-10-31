using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IHealth
{
	[SerializeField] protected bool showDebugLog = false;

	//**************************************************\\
	//********************* Fields *********************\\
	//**************************************************\\
	[SerializeField] private int _maxHealth;
	[SerializeField] private int _currentHealth;
	[SerializeField] private float _timeToDie;
	private bool _isDead;
	private bool _isHurt;

	//**************************************************\\
	//******************** Methods *********************\\
	//**************************************************\\

	public virtual void TakeDamage(IDamage damage) {
		if (_isDead) {
			return;
		}
		_currentHealth -= damage.DamageAmount;
		_isHurt = true;

		if (_currentHealth <= 0) {
			Die();
			return;
		} else {
			StartCoroutine(FadeHurt());
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
		// Log
		if (showDebugLog) {
			Debug.Log(gameObject.name + " has died!");
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