using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour, IInput
{
	[SerializeField] protected bool _showDebugLog = false;

	//**************************************************\\
	//********************* Fields *********************\\
	//**************************************************\\

	public float _horizontalInput;
	public float _verticalInput;
	public bool _jumpInput;
	public bool _dashInput;
	public bool _interactInput;
	public bool _weaponInput;
	//**************************************************\\
	//******************** Methods *********************\\
	//**************************************************\\

	private void Update() {
		_horizontalInput = Input.GetAxisRaw("Horizontal");
		_verticalInput   = Input.GetAxisRaw("Vertical");
		_jumpInput       = Input.GetKey(KeyCode.Space);
		_dashInput       = Input.GetKey(KeyCode.LeftShift);
		_interactInput   = Input.GetKeyDown(KeyCode.E);
		_weaponInput     = Input.GetKey(KeyCode.Mouse0);
	}
	//**************************************************\\
	//******************* Properties *******************\\
	//**************************************************\\
	public float HorizontalInput { get { return _horizontalInput; } }
	public float VerticalInput { get { return _verticalInput; } }
	public bool JumpInput { get { return _jumpInput; } }
	public bool DashInput { get { return _dashInput; } }
	public bool InteractInput { get { return _interactInput; } }
	public bool WeaponInput { get { return _weaponInput; } }
}
