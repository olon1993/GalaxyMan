using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana
{
	public interface IBossAction
	{
		string ActionName { get; }
		bool ShowDebugLog { get; }
		BossManager BossManagerScript { get; }
		bool ActionBusy { get; }
		bool ActionDelay { get; }
		IActionLocations[] StartLocations { get; }
		IActionLocations[] EndLocations { get; }
		bool CanEndOnSameLocation { get; }
		bool MustEndOnSameLocation { get; }
		float[] AnticipationTime { get; }
		float[] ActionTime { get; }
		float[] RecoveryTime { get; }
		GameObject AnticipationEffect { get; }
		int StartLocationId { get; }
		int EndLocationId { get; }
		int StartInt { get; }
		int EndInt { get; }
		bool PhaseChangeAction { get; }
		bool CheckActionPossibility(int start);
		void RunAction(int phase);
	}
}