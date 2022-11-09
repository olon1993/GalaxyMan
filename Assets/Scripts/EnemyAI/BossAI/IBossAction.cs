using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBossAction
{
	string ActionName { get; }
	bool ShowDebugLog { get; }
	BossManager BossManager { get; }
	bool ActionBusy { get; }
	bool ActionDelay { get; }
	IActionLocations[] StartLocations { get; }
	IActionLocations[] EndLocations { get; }
	bool CanEndOnSameLocation { get; }
	bool MustEndOnSameLocation { get; }
	float TotalActionTime { get; }
	float AnticipationTime { get; }
	float ActionDelayTime { get; }
	GameObject AnticipationEffect { get; }
	int StartLocationId { get; }
	int EndLocationId { get; }
	int StartInt { get; }
	int EndInt { get; }
	bool CheckActionPossibility(int start);
	void RunAction();
}
