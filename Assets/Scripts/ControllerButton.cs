﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]
public class ControllerButton : MonoBehaviour {

	bool buttonDown;
	public string name;

	private int pointerIndex = -1;
	private BoxCollider2D collider;

	void Start() {
		collider = GetComponent<BoxCollider2D> ();
	}
	

	void Update() {
		Touch[] touches = Input.touches;
		foreach (Touch touch in touches) {
			CheckForTouch(touch);
		}
	}

	void CheckForTouch(Touch touch) {
		TouchPhase touchPhase = touch.phase;
		int pointer = touch.fingerId;
		Vector2 pos = touch.position;
		bool contains = ContainsTouch (pos);
		if (pointerIndex == pointer && (touchPhase == TouchPhase.Ended)) {
			buttonDown = false;
			pointerIndex = -1;
		}
		if (pointerIndex == pointer && touchPhase == TouchPhase.Moved && !contains) {
			buttonDown = false;
		}
		if (contains && (touchPhase == TouchPhase.Moved || touchPhase == TouchPhase.Began || touchPhase == TouchPhase.Stationary)) {
			buttonDown = true;
			pointerIndex = pointer;
		}
	}

	bool ContainsTouch(Vector2 pos) {
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, Camera.main.transform.position.z - transform.position.z ));
		worldPos.z = collider.bounds.center.z;
		return collider.bounds.Contains (worldPos);
	}

	public bool GetButtonDown() {
		return buttonDown;
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.cyan;

			Gizmos.DrawWireCube(collider2D.bounds.center, collider2D.bounds.size);
	}

}
