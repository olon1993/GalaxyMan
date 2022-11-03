using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInput
{
	float HorizontalInput { get; } // - A D
	float VerticalInput { get; } // - W S
	bool JumpInput { get; } // -space
	bool DashInput { get; } // -shift
	bool InteractInput { get; } // - E
	bool WeaponInput { get; } // - Mouse 0
}
