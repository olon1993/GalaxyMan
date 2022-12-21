using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheFrozenBanana
{
    public class Combatant : MonoBehaviour, ICombatant
    {
        [SerializeField] private bool _showDebugLog = false;

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        private DependencyManager _dependencyManager;

        private IInputManager _inputManager;
        private ILocomotion2dSideScroller _locomotion;
        private IHealth _health;

        [SerializeField] private IWeapon.WeaponType _mainWeaponType = IWeapon.WeaponType.RANGED;
        private IList<IWeapon> _mainWeapons;
        private IWeapon _currentMainWeapon;
        private int _currentMainWeaponIndex = 0;

        private IList<IWeapon> _secondaryWeapons;
        private IWeapon _currentSecondaryWeapon;
        private int _currentSecondaryWeaponIndex = 0;


        private bool _isCycleWeaponsEnabled = true;

        private int _horizontalFacingDirection = 1;

        private bool _isCharging = false;
        private bool includeSeconary = false;
        private float _attackChargeTime = 0f;
        private Vector3 _combatDirection = Vector3.right;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        void Awake() {

            if (_showDebugLog) {
                Debug.Log("Combatant Debug Log is on");
            }
            _dependencyManager = GetComponent<DependencyManager>();
            if (_dependencyManager == null) {
                Debug.Log("No DependencyManager found on " + name);
            }

            _inputManager = (IInputManager)_dependencyManager.Registry[typeof(IInputManager)];
            if (_inputManager == null) {
                Debug.Log("No Input Manager found on " + name);
            }

            _health = (IHealth)_dependencyManager.Registry[typeof(IHealth)];
            if (_health == null) {
                Debug.LogError("IHealth not found on " + gameObject.name);
            }

            _locomotion = (ILocomotion2dSideScroller)_dependencyManager.Registry[typeof(ILocomotion2dSideScroller)];
            if (_health == null) {
                Debug.LogError("IHealth not found on " + gameObject.name);
            }

            _mainWeapons = new List<IWeapon>();
            _secondaryWeapons = new List<IWeapon>();

            IList<IWeapon> allWeapons = transform.GetComponentsInChildren<IWeapon>().ToList();

            foreach (IWeapon weapon in allWeapons) {
                if (weapon.WeaponTypeDefinition == _mainWeaponType) {
                    _mainWeapons.Add(weapon);
                } else {
                    _secondaryWeapons.Add(weapon);
                }
            }

            if (_mainWeapons.Count > 0) {
                _currentMainWeapon = _mainWeapons[0];
            }

            if (_secondaryWeapons.Count > 0) {
                _currentSecondaryWeapon = _secondaryWeapons[0];
                includeSeconary = true;
            }
            if (_showDebugLog) {
                Debug.Log("Main      Weapons Count: " + _mainWeapons.Count);
                Debug.Log("Secondary Weapons Count: " + _secondaryWeapons.Count);
            }
        }

        private void Update()
        {
            IsAttack = _inputManager.IsAttack;
            IsAttacking = _inputManager.IsAttacking;
            IsSecondaryAttack = _inputManager.IsSecondaryAttack;
            IsCycleWeapons = _inputManager.IsToggleWeapons;
            if (CurrentMainWeapon.IsAttacking) {
                IsSecondaryAttack = false;
			} else if (includeSeconary) {
                if (CurrentSecondaryWeapon.IsAttacking) {
                    IsAttack = false;
                    IsAttacking = false;
                }
            }

            bool isUp = _inputManager.Vertical > Mathf.Epsilon;

            if (IsAttacking && !_isCharging) {
                CurrentMainWeapon.ChargeEffect();
                _isCharging = true;
			}

            if (isUp)
            {
                CurrentMainWeapon.PointOfOrigin.localPosition = new Vector3(0, 1.5f, 0);
                CurrentMainWeapon.PointOfTargetting.localPosition = new Vector3(0, 5f, 0);
            }
            else if (_locomotion.IsWallSliding)
            {

                CurrentMainWeapon.PointOfOrigin.localPosition = new Vector3(-1, 0, 0);
                CurrentMainWeapon.PointOfTargetting.localPosition = new Vector3(-5, 0, 0);
            }
            else 
            {
                CurrentMainWeapon.PointOfOrigin.localPosition = new Vector3(_horizontalFacingDirection, 0, 0);
                CurrentMainWeapon.PointOfTargetting.localPosition = new Vector3(_horizontalFacingDirection * 5, 0, 0);
            }

            if (IsAttacking)
            {
                _attackChargeTime += Time.deltaTime;
            }

            if (IsAttack)
            {
                CurrentMainWeapon.Attack(_attackChargeTime);
                _attackChargeTime = 0f;
                _isCharging = false;
                return;
            }
            if (IsSecondaryAttack) {
                _attackChargeTime = 0f;
                _isCharging = false;
                _currentSecondaryWeapon.Attack(_attackChargeTime);
                return;
			}

            CheckWeaponToggle();
        }

        private void CheckWeaponToggle()
        {
            if (IsCycleWeaponsEnabled)
            {
                if (IsCycleWeapons)
                {
                    _currentMainWeaponIndex++;
                    if (_currentMainWeaponIndex >= _mainWeapons.Count) 
                    { 
                        _currentMainWeaponIndex = 0; 
                    }
                    if (_isCharging) {
                        CurrentMainWeapon.Attack(_attackChargeTime);
                        _attackChargeTime = 0f;
                        _isCharging = false;
                    }
                    CurrentMainWeapon = _mainWeapons[_currentMainWeaponIndex];
                }

                if (IsHotKeyOne && IsHotKeyOneEnabled)
                {
                    _currentMainWeaponIndex = 0;
                    CurrentMainWeapon = _mainWeapons[_currentMainWeaponIndex];
                }

                if (IsHotKeyTwo && IsHotKeyTwoEnabled)
                {
                    _currentMainWeaponIndex = 1;
                    CurrentMainWeapon = _mainWeapons[_currentMainWeaponIndex];
                }

                IsCycleWeapons = false;
                IsHotKeyOne = false;
                IsHotKeyTwo = false;
            }
        }

        //**************************************************\\
        //******************* Properties *******************\\
        //**************************************************\\

        public IHealth Health
        {
            get { return _health; }
            set
            {
                if (_health != value)
                {
                    _health = value;
                }
            }
        }

        public IList<IWeapon> MainWeapons
        {
            get { return _mainWeapons; }
            set
            {
                if (_mainWeapons != value)
                {
                    _mainWeapons = value;
                }
            }
        }

        public IWeapon CurrentMainWeapon
        {
            get { return _currentMainWeapon; }
            set
            {
                if (_currentMainWeapon != value)
                {
                    _currentMainWeapon = value;
                }
            }
        }

        public IWeapon CurrentSecondaryWeapon {
            get { return _currentSecondaryWeapon; }
            set {
                if (_currentSecondaryWeapon != value) {
                    _currentSecondaryWeapon = value;
                }
            }
        }

        public bool IsAttack { get; set; }

        public bool IsAttacking { get; set; }

        public bool IsSecondaryAttack { get; set; }

        public int HorizontalFacingDirection
        {
            get { return _horizontalFacingDirection; }
            set
            {
                if (_horizontalFacingDirection != value && value != 0)
                {
                    _horizontalFacingDirection = value;
                    if (Mathf.Sign(CurrentMainWeapon.PointOfOrigin.localPosition.x) != Mathf.Sign(_horizontalFacingDirection))
                    {
                        CurrentMainWeapon.PointOfOrigin.transform.localPosition *= -1;
                        CurrentMainWeapon.PointOfTargetting.transform.localPosition *= -1;
                    }
                }
            }
        }

        public bool IsCycleWeaponsEnabled { get { return _isCycleWeaponsEnabled; } set { _isCycleWeaponsEnabled = value;  } }
        public bool IsCycleWeapons { get; set; }
        public bool IsHotKeyOne { get; set; }
        public bool IsHotKeyOneEnabled { get; set; }
        public bool IsHotKeyTwo { get; set; }
        public bool IsHotKeyTwoEnabled { get; set; }

    }
}
