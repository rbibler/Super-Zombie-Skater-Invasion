using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animator))]
public class Skater : MonoBehaviour {


	public float skateAccel;
	public float jumpAccel;
	public GameValues gameValues;

	private int state;
	private bool down;
	private bool space;
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
	}

	void SetAnimState() {
		animator.SetInteger ("state", state);
	}

	public void SetInput(bool down, bool space) {
		this.down = down;
		this.space = space;
	}

	void HandleInput() {
		switch (state) {
		case STATE_SKATING:
			if (space) {
				Jump ();
			}
			else if (down) {
				state = STATE_CROUCHING;
			}
			break;
		case STATE_JUMPING: 
			break;
		}
	}
	
	void Jump() {
		print ("Jumping");
		state = STATE_JUMPING;
		transform.rigidbody2D.velocity = Vector3.up * jumpAccel;
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
