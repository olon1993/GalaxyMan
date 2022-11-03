using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossObsoleteScript : MonoBehaviour
{
	//**************************************************\\
	//********************* Fields *********************\\
	//**************************************************\\


	[SerializeField] protected bool _showDebugLog = false;

	[SerializeField] protected GameObject rndrr;
	private Vector3 baseScale;

	private Transform target;
	private bool acting;
	private bool shooting;
	private bool justJumped;
	// private bool shootPossible;
	private bool jumpPossible;
	private bool superJumpPossible;
	private bool quickMovePossible;
	private bool chargePossible;
	//////////////////////////////////////////////////////
	public GameObject[] locations;
	private int currentLocation = 0;
	protected float _direction = -1f;

	private int[][] jumpPaths;
	private int[][] superJumpPaths;
	private int[][] quickMoves;

	//**************************************************\\
	//******************** Methods *********************\\
	//**************************************************\\
	private void Awake() {
		baseScale = rndrr.transform.localScale;
		SetupPaths();
		SetupSuperJump();
		target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
	}

	private void Update() {
		if (!shooting) {
			_direction = Mathf.Sign(target.position.x - transform.position.x);
			rndrr.transform.localScale = new Vector3(baseScale.x * _direction, baseScale.y, baseScale.z);
		}
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
		DecideAction();

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

	private void DecideAction() {
		float decide = Random.Range(0f, 1f);

		if (jumpPossible && quickMovePossible) {
			if (decide > 0.6f) {
				StartCoroutine(RunQuickMove());
			} else {
				StartCoroutine(RunJumpAction());
			}
		} else if (superJumpPossible && jumpPossible) {
			if (decide > 0.6f) {
				StartCoroutine(RunSuperJumpAction());
			} else {
				StartCoroutine(RunJumpAction());
			}


		} else if (jumpPossible) {
			StartCoroutine(RunJumpAction());
		}
	}

	//**************************************************\\
	//********************* JUMP ***********************\\
	//**************************************************\\
	private IEnumerator RunJumpAction() {

		if (_showDebugLog) {
			Debug.Log("Jump");
		}
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
		int endLoc = jumpPaths[startIterations[jumpSelect]][1];
		float t = 0f;
		// Jump math
		float startX = locations[startLoc].transform.position.x;
		float startY = locations[startLoc].transform.position.y;
		float endX = locations[endLoc].transform.position.x;
		float endY = locations[endLoc].transform.position.y;
		Vector2 start = new Vector2(startX, startY);
		Vector2 end = new Vector2(endX, endY);
		float jumpY = 10f;
		while (t < 2f) {
			yield return new WaitForEndOfFrame();
			t += Time.deltaTime;

			float i = t / 2;
			float heightY = jumpY * Mathf.Sin(i * Mathf.PI);
			Vector2 bossLoc = Vector2.Lerp(start, end, i) + new Vector2(0,heightY);
			transform.position = bossLoc;
		}
		transform.position = end;
		currentLocation = endLoc;
		yield return new WaitForSeconds(1f);
		acting = false;
	}


	//**************************************************\\
	//****************** SUPER JUMP ********************\\
	//**************************************************\\
	private IEnumerator RunSuperJumpAction() {

		if (_showDebugLog) {
			Debug.Log("Super Jump");
		}
		yield return new WaitForEndOfFrame();
		int startLoc = currentLocation;
		int endLoc = 2;
		float t = 0f;
		// Jump math
		float startX = locations[startLoc].transform.position.x;
		float startY = locations[startLoc].transform.position.y;
		float endX = locations[endLoc].transform.position.x;
		float endY = locations[endLoc].transform.position.y;
		Vector2 start = new Vector2(startX, startY);
		Vector2 end = new Vector2(endX, endY);
		float jumpY = 20f;
		while (t < 2f) {
			yield return new WaitForEndOfFrame();
			t += Time.deltaTime;

			if (t < 1) {
				float heightY = jumpY * Mathf.Sin(t / 2 * Mathf.PI);
				Vector2 bossLoc = Vector2.Lerp(start, end, t) + new Vector2(0, heightY);
				transform.position = bossLoc;
			} else {
				Vector2 bossLoc = end + new Vector2(0,jumpY) * (2-t);
				transform.position = bossLoc;
			}
		}
		transform.position = end;
		currentLocation = endLoc;
		yield return new WaitForSeconds(1f);
		acting = false;
	}

	//**************************************************\\
	//****************** QUICK MOVE ********************\\
	//**************************************************\\
	private IEnumerator RunQuickMove() {
		if (_showDebugLog) {
			Debug.Log("Quick Move");
		}
		yield return new WaitForEndOfFrame();
		int startLoc = currentLocation;
		int[] startIterations = new int[0];
		int count = 0;
		for (int i = 0; i < quickMoves.Length; i++) {
			if (quickMoves[i][0] == startLoc) {
				count++;
				int[] tmp = new int[count];
				for (int j = 0; j < tmp.Length - 1; j++) {
					tmp[j] = startIterations[j];
				}
				tmp[count - 1] = i;
				startIterations = tmp;
			}
		}
		// now the matrix of start/end is setup in startIterations, choose a random number, and get its end location from jump paths
		int moveSelect = Random.Range(0, startIterations.Length);
		int endLoc = quickMoves[startIterations[moveSelect]][1];
		float t = 0f;
		// Jump math
		float startX = locations[startLoc].transform.position.x;
		float startY = locations[startLoc].transform.position.y;
		float endX = locations[endLoc].transform.position.x;
		float endY = locations[endLoc].transform.position.y;
		Vector2 start = new Vector2(startX, startY);
		Vector2 end = new Vector2(endX, endY);
		while (t < 1f) {
			yield return new WaitForEndOfFrame();
			t += Time.deltaTime;

			Vector2 bossLoc = Vector2.Lerp(start, end, t);
			transform.position = bossLoc;
		}
		transform.position = end;
		currentLocation = endLoc;
		yield return new WaitForSeconds(1f);
		acting = false;
	}

	//**************************************************\\
	//********************* SHOOT **********************\\
	//**************************************************\\
	private IEnumerator RunShootAction() {
		yield return new WaitForEndOfFrame();
		// point towards target
		float ver = Mathf.Sign(target.position.y - transform.position.y);

		/*
		    5 shots, going up 15 degrees each shot
			vertical up means start 0 degrees, 
			vertical down means start -45 degrees, 
		 */
		yield return new WaitForSeconds(1f);
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
		quickMoves[0][0] = 2;
		quickMoves[0][1] = 0;
		quickMoves[1][0] = 2;
		quickMoves[1][1] = 4;
	}
}
