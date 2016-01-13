﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animator))]
[RequireComponent (typeof (Skater))]
public class Skater : MonoBehaviour {


	public float skateAccel;
	public float jumpAccel;
	public float jumpAccelIncrease;
	public float speed;
	public float scoreMultiplier;
	public GameValues gameValues;
	public GameLoopManager gameLoop;
	public HUDManager hud;

	private int state;
	private float potentialJumpAccel = 6;
	private bool down;
	private bool space;
	private bool spaceUp;
	private Animator animator;
	private static int lives = 3;
	private static float score;


	public const int STATE_SKATING = 0x00;
	public const int STATE_CROUCHING = 0x01;
	public const int STATE_JUMPING = 0x02;
	public const int STATE_BAILING = 0x03;
	
	// Use this for initialization
	void Start () {
		state = STATE_SKATING;
		animator = GetComponent<Animator> ();
		hud.UpdateLives(lives);
		gameValues.speed = speed;
	}
	
	// Update is called once per frame
	void Update () {
		HandleInput ();
		SetAnimState ();
		UpdateScore();
	}

	void SetAnimState() {
		animator.SetInteger ("state", state);
	}

	public void SetInput(bool down, bool space, bool spaceUp) {
		this.down = down;
		this.space = space;
		this.spaceUp = spaceUp;
	}

	void HandleInput() {
		switch (state) {
		case STATE_SKATING:
			if(space) {
				Crouch(true);
			}
			break;
		case STATE_CROUCHING:
			if(space) {
				Crouch(true);
			} else if(spaceUp) {
				Jump ();
			}
			break;
		case STATE_JUMPING: 
			if(space) {
				Crouch (false);
			}
			break;
		}
	}
	
	void Crouch(bool changeAnimState) {
		potentialJumpAccel += jumpAccelIncrease * Time.deltaTime;
		if(potentialJumpAccel >= jumpAccel) {
			potentialJumpAccel = jumpAccel;
		}
		if(changeAnimState) {
			state = STATE_CROUCHING;
		}
	}
	
	void Jump() {
		state = STATE_JUMPING;
		transform.rigidbody2D.velocity = Vector3.up * potentialJumpAccel;
		potentialJumpAccel = 6;
	}
	
	void UpdateScore() {
		score += (scoreMultiplier * -gameValues.speed * Time.deltaTime);
		hud.UpdateScore(score);
	}

	void OnCollisionEnter2D(Collision2D col) {
		Vector2 contactNormal = col.contacts [0].normal;
		if (contactNormal.normalized.y == 1.0) {
			state = STATE_SKATING;
		} else if (contactNormal.normalized.x == -1.0 && state != STATE_BAILING) {
			state = STATE_BAILING;
			gameValues.SetSpeed(0);
		}
	}

	void StandUp() {
		state = STATE_SKATING;
		gameValues.SetSpeed(speed);
	}
	
	public void Die() {
		lives--;
		hud.UpdateLives(lives);
		gameLoop.Die (GameLoopManager.DEATH_RELOAD);
		Destroy (gameObject);
	}
}
