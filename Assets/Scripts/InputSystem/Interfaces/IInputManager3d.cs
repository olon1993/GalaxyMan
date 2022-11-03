using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public interface IInputManager3d : IInputManager
	{
		bool IsStrafe { get; }
		bool IsSwitchWeapon { get; }
		bool IsRunning { get; }
		Vector3 CurrentTarget { get; }
	}
}