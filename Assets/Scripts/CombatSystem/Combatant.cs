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

        private IList<IWeapon> _weapons;
        private IWeapon _currentWeapon;
        private int _currentWeaponIndex = 0;

        private int _horizontalFacingDirection = 1;

        private float _attackChargeTime = 0f;
        private Vector3 _combatDirection = Vector3.right;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        void Awake()
        {
            _dependencyManager = GetComponent<DependencyManager>();
            if(_dependencyManager == null)
            {
                Debug.Log("No DependencyManager found on " + name);
            }

            _inputManager = (IInputManager)_dependencyManager.Registry[typeof(IInputManager)];
            if (_inputManager == null)
            {
                Debug.Log("No Input Manager found on " + name);
            }

            _health = (IHealth)_dependencyManager.Registry[typeof(IHealth)];
            if (_health == null)
            {
                Debug.LogError("IHealth not found on " + gameObject.name);
            }

            _locomotion = (ILocomotion2dSideScroller)_dependencyManager.Registry[typeof(ILocomotion2dSideScroller)];
            if (_health == null)
            {
                Debug.LogError("IHealth not found on " + gameObject.name);
            }

            _weapons = new List<IWeapon>();
            IList<IWeapon> weapons = transform.GetComponentsInChildren<IWeapon>().ToList();
            foreach (IWeapon weapon in weapons)
            {
                _weapons.Add(weapon);
            }

            if(_weapons.Count > 0)
            {
                _currentWeapon = _weapons[0];
            }
        }

        private void Update()
        {
            IsAttack = _inputManager.IsAttack;
            IsAttacking = _inputManager.IsAttacking;

            bool isUp = _inputManager.Vertical > Mathf.Epsilon;

            if (isUp)
            {
                CurrentWeapon.PointOfOrigin.localPosition = new Vector3(0, 1.5f, 0);
                CurrentWeapon.PointOfTargetting.localPosition = new Vector3(0, 5f, 0);
            }
            else if (_locomotion.IsWallSliding)
            {

                CurrentWeapon.PointOfOrigin.localPosition = new Vector3(-1, 0, 0);
                CurrentWeapon.PointOfTargetting.localPosition = new Vector3(-5, 0, 0);
            }
            else 
            {
                CurrentWeapon.PointOfOrigin.localPosition = new Vector3(_horizontalFacingDirection, 0, 0);
                CurrentWeapon.PointOfTargetting.localPosition = new Vector3(_horizontalFacingDirection * 5, 0, 0);
            }

            if (IsAttacking)
            {
                _attackChargeTime += Time.deltaTime;
            }

            if (IsAttack)
            {
                CurrentWeapon.Attack(_attackChargeTime);
                _attackChargeTime = 0f;
            }

            CheckWeaponToggle();
        }

        private void CheckWeaponToggle()
        {
            if (IsCycleWeaponsEnabled)
            {
                if (IsCycleWeapons)
                {
                    _currentWeaponIndex++;
                    if (_currentWeaponIndex >= _weapons.Count) 
                    { 
                        _currentWeaponIndex = 0; 
                    }

                    CurrentWeapon = _weapons[_currentWeaponIndex];
                }

                if (IsHotKeyOne && IsHotKeyOneEnabled)
                {
                    _currentWeaponIndex = 0;
                    CurrentWeapon = _weapons[_currentWeaponIndex];
                }

                if (IsHotKeyTwo && IsHotKeyTwoEnabled)
                {
                    _currentWeaponIndex = 1;
                    CurrentWeapon = _weapons[_currentWeaponIndex];
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

        public IList<IWeapon> Weapons
        {
            get { return _weapons; }
            set
            {
                if (_weapons != value)
                {
                    _weapons = value;
                }
            }
        }

        public IWeapon CurrentWeapon
        {
            get { return _currentWeapon; }
            set
            {
                if (_currentWeapon != value)
                {
                    _currentWeapon = value;
                }
            }
        }

        public bool IsAttack { get; set; }

        public bool IsAttacking { get; set; }

        public int HorizontalFacingDirection
        {
            get { return _horizontalFacingDirection; }
            set
            {
                if (_horizontalFacingDirection != value && value != 0)
                {
                    _horizontalFacingDirection = value;
                    if (Mathf.Sign(CurrentWeapon.PointOfOrigin.localPosition.x) != Mathf.Sign(_horizontalFacingDirection))
                    {
                        CurrentWeapon.PointOfOrigin.transform.localPosition *= -1;
                        CurrentWeapon.PointOfTargetting.transform.localPosition *= -1;
                    }
                }
            }
        }

        public bool IsCycleWeaponsEnabled { get; set; }
        public bool IsCycleWeapons { get; set; }
        public bool IsHotKeyOne { get; set; }
        public bool IsHotKeyOneEnabled { get; set; }
        public bool IsHotKeyTwo { get; set; }
        public bool IsHotKeyTwoEnabled { get; set; }

    }
}
