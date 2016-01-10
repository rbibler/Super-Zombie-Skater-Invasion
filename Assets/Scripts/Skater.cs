using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animator))]
public class Skater : MonoBehaviour {


	public float skateAccel;
	public float jumpAccel;
	public float jumpAccelIncrease;
	public GameValues gameValues;

	private int state;
	private float potentialJumpAccel = 6;
	private bool down;
	private bool space;
	private bool spaceUp;
	private Animator animator;


	public const int STATE_SKATING = 0x00;
	public const int STATE_CROUCHING = 0x01;
	public const int STATE_JUMPING = 0x02;
	public const int STATE_BAILING = 0x03;
	
	// Use this for initialization
	void Start () {
		state = STATE_SKATING;
		animator = GetComponent<Animator> ();

	}
	
	// Update is called once per frame
	void Update () {
		HandleInput ();
		SetAnimState ();
		print(state);
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
		print(potentialJumpAccel);
		if(potentialJumpAccel >= jumpAccel) {
			potentialJumpAccel = jumpAccel;
		}
		if(changeAnimState) {
			state = STATE_CROUCHING;
		}
	}
	
	void Jump() {
		print ("Jumping");
		state = STATE_JUMPING;
		transform.rigidbody2D.velocity = Vector3.up * potentialJumpAccel;
		potentialJumpAccel = 6;
	}

	void OnCollisionEnter2D(Collision2D col) {
		Vector2 contactNormal = col.contacts [0].normal;
		print (contactNormal);
		if (contactNormal.normalized.y == 1.0) {
			print ("hit ground");
			state = STATE_SKATING;
		} else if (contactNormal.normalized.x == -1.0 && state != STATE_BAILING) {
			state = STATE_BAILING;
			gameValues.SetSpeed(0);
		}
	}

	void StandUp() {
		state = STATE_SKATING;
		gameValues.ResetSpeed ();
	}
	
	
}
