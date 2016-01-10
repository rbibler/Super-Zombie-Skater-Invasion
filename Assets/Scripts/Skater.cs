using UnityEngine;
using System.Collections;

public class Skater : MonoBehaviour {


	public float skateAccel;
	public float jumpAccel;
	public Vector2 vel;
	public Vector3 pos;
	
	public float width;
	public float height;

	private int state;
	private bool down;
	private bool left;
	private bool right;
	private bool space;

	public const int STATE_IDLE = 0x00;
	public const int STATE_SKATING = 0x01;
	public const int STATE_JUMPING = 0x02;
	public const int STATE_CROUCHING = 0x03;
	public const int STATE_FALLING = 0x04;
	
	// Use this for initialization
	void Start () {
		vel = rigidbody2D.velocity;
		pos = transform.position;
		SetWidthHeight();
		state = STATE_IDLE;
		//this.rigidbody2D.fixedAngle = true;
	}
	
	void SetWidthHeight() {
		width = GetComponent<SpriteRenderer>().bounds.size.x;
		height = GetComponent<SpriteRenderer>().bounds.size.y;
	}
	
	// Update is called once per frame
	void Update () {
		vel.x = 0;
		vel.y = rigidbody2D.velocity.y;
		pos = transform.position;
		HandleInput ();
		//RestrictBounds();
		this.rigidbody2D.velocity = vel;
	}

	public void SetInput(bool down, bool left, bool right, bool space) {
		this.down = down;
		this.left = left;
		this.right = right;
		this.space = space;
	}

	void HandleInput() {
		switch (state) {
		case STATE_IDLE: 
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
	
	void RestrictBounds() {
		float futurePos = pos.x + vel.x;
		if(futurePos <= 0) {
			vel.x = 0;
		} else if(futurePos >= 26) {
			vel.x = 0;
		}
	}
	
	void Jump() {
		state = STATE_JUMPING;
		vel.y = jumpAccel;
		transform.rotation.Set(.25f, 0, 0, 0);
	}

	void OnCollisionEnter2D(Collision2D col) {
		state = STATE_SKATING;
	}
	
	
}
