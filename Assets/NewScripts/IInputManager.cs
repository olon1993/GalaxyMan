using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputManager
{
	float Horizontal { get; }
	float Vertical { get; }
	bool IsJump { get; }
	bool IsJumpCancelled { get; }
	bool IsDash { get; }
	bool IsAttack { get; }

}

