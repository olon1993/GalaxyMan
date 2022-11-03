using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TheFrozenBanana
{
	public class InputManager3d : MonoBehaviour, IInputManager3d
	{
		//**************************************************\\
		//********************* Fields *********************\\
		//**************************************************\\

		protected float _horizontal;
		protected float _vertical;
		protected bool _isStrafe;
		protected bool _isDash;
		protected bool _isJump;
		protected bool _isAttack;
		protected bool _isSwitchWeapon;
		protected bool _isRunning;
		protected bool _isEnabled;
		protected bool _isToggleInventory;

		//**************************************************\\
		//******************** Methods *********************\\
		//**************************************************\\

		protected virtual void Update()
		{
			_isStrafe = Input.GetKey(KeyCode.LeftShift);
			_isJump = Input.GetKey(KeyCode.Space);
			_vertical = Input.GetAxis("Vertical");
			_horizontal = Input.GetAxis("Horizontal");
			_isDash = Input.GetKeyDown(KeyCode.LeftControl);
			_isAttack = Input.GetKeyDown(KeyCode.Mouse0);
			_isSwitchWeapon = Input.GetKeyDown(KeyCode.F);
			_isToggleInventory = Input.GetKeyDown(KeyCode.I);
			if (Input.GetKeyDown(KeyCode.T))
			{
				_isRunning = !_isRunning;
			}
		}

		//**************************************************\\
		//******************* Properties *******************\\
		//**************************************************\\

		public float Horizontal { get { return _horizontal; } }
		public float Vertical { get { return _vertical; } }
		public bool IsStrafe { get { return _isStrafe; } }
		public bool IsDash { get { return _isDash; } }
		public bool IsJump { get { return _isJump; } }
		public bool IsAttack { get { return _isAttack; } }
		public bool IsSwitchWeapon { get { return _isSwitchWeapon; } }
		public bool IsRunning { get { return _isRunning; } }

		public bool IsToggleInventory { get { return _isToggleInventory; } }
		public bool IsEnabled
		{
			get { return _isEnabled; }
			set { _isEnabled = value; }
		}

		public bool IsJumpCancelled => throw new System.NotImplementedException();

		public Vector3 CurrentTarget => throw new System.NotImplementedException();
	}
}
