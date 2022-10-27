using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
	//**************************************************\\
	//********************* Fields *********************\\
	//**************************************************\\


	private bool acting;
	private bool justJumped;
	// private bool shootPossible;
	private bool jumpPossible;
	private bool superJumpPossible;
	private bool quickMovePossible;
	//////////////////////////////////////////////////////
	private GameObject[] locations;
	private int currentLocation = 0;

	private int[][] jumpPaths;
	private int[][] superJumpPaths;
	private int[][] quickMoves;

	//**************************************************\\
	//******************** Methods *********************\\
	//**************************************************\\
	private void Awake() {
		SetupPaths();
		SetupSuperJump();
	}

	private void Update() {
		if (!acting) {
			StartCoroutine(SelectAction());
		}
	}

	private IEnumerator SelectAction() {
		acting = true;
		jumpPossible = false;
		superJumpPossible = false;
		quickMovePossible = false;
		yield return new WaitForEndOfFrame();
		WhatActionsAreAvailable();
		if (jumpPossible) {
			StartCoroutine(RunJumpAction());
		}
	}

	private void WhatActionsAreAvailable() {
		for (int i = 0; i < jumpPaths.Length; i++) {
			if (jumpPaths[i][0] == currentLocation) {
				jumpPossible = true;
				break;
			}
		}
		for (int i = 0; i < superJumpPaths.Length; i++) {
			if (superJumpPaths[i][0] == currentLocation) {
				superJumpPossible = true;
				break;
			}
		}
		for (int i = 0; i < quickMoves.Length; i++) {
			if (quickMoves[i][0] == currentLocation) {
				quickMovePossible = true;
				break;
			}
		}
	}

	private IEnumerator RunJumpAction() {
		yield return new WaitForEndOfFrame();
		int startLoc = currentLocation;
		int[] startIterations = new int[0];
		int count = 0;
		for (int i = 0; i < jumpPaths.Length; i++) {
			if (jumpPaths[i][0] == startLoc) {
				count++;
				int[] tmp = new int[count];
				for (int j = 0; j < tmp.Length-1; j++) {
					tmp[j] = startIterations[j];
				}
				tmp[count-1] = i;
				startIterations = tmp;
			}
		}
		// now the matrix of start/end is setup in startIterations, choose a random number, and get its end location from jump paths

		int jumpSelect = Random.Range(0, startIterations.Length);
		Debug.Log("Jump Selected: " + jumpSelect);
		acting = false;
	}

	//**************************************************\\
	//********************* SETUP **********************\\
	//**************************************************\\
	private void SetupPaths() {
		jumpPaths = new int[10][];
		for (int i = 0; i < jumpPaths.Length; i++) {
			jumpPaths[i] = new int[2];
		}
		jumpPaths[0][0] = 0;
		jumpPaths[0][1] = 1;
		jumpPaths[1][0] = 1;
		jumpPaths[1][1] = 0;
		jumpPaths[2][0] = 1;
		jumpPaths[2][1] = 2;
		jumpPaths[3][0] = 1;
		jumpPaths[3][1] = 3;
		jumpPaths[4][0] = 2;
		jumpPaths[4][1] = 1;
		jumpPaths[5][0] = 2;
		jumpPaths[5][1] = 3;
		jumpPaths[6][0] = 3;
		jumpPaths[6][1] = 1;
		jumpPaths[7][0] = 3;
		jumpPaths[7][1] = 2;
		jumpPaths[8][0] = 3;
		jumpPaths[8][1] = 4;
		jumpPaths[9][0] = 4;
		jumpPaths[9][1] = 3;
	}

	private void SetupSuperJump() {
		superJumpPaths = new int[2][];
		for (int i = 0; i < superJumpPaths.Length; i++) {
			superJumpPaths[i] = new int[2];
		}
		superJumpPaths[0][0] = 1;
		superJumpPaths[0][1] = 2;
		superJumpPaths[1][0] = 3;
		superJumpPaths[1][1] = 2;
		quickMoves = new int[2][];
		for (int i = 0; i < quickMoves.Length; i++) {
			quickMoves[i] = new int[2];
		}
		quickMoves[0][0] = 1;
		quickMoves[0][1] = 2;
		quickMoves[1][0] = 3;
		quickMoves[1][1] = 2;
	}
}
