using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBossAction
{
	string ActionName { get; }
	bool ShowDebugLog { get; }
	bool ActionBusy { get; }
	IActionLocations[] StartLocations { get; }
	IActionLocations[] EndLocations { get; }
	bool CanEndOnSameLocation { get; }
	bool MustEndOnSameLocation { get; }
	float TotalActionTime { get; }
	int StartLocationId { get; }
	int EndLocationId { get; }
	int StartInt { get; }
	int EndInt { get; }
	bool CheckActionPossibility(int start);
	void RunAction();
}
