using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animator))]
[RequireComponent (typeof (Skater))]
public class Skater : MonoBehaviour {


	public float skateAccel;
	public float jumpAccel;
	public float jumpAccelIncrease;
	public float speed;
	public float scoreMultiplier;
	public float maxVerticalVelocity;
	public float slamRegenTime;
	public float slamSpeed;
	public GameValues gameValues;
	public GameLoopManager gameLoop;
	public HUDManager hud;

	private int state;
	private float potentialJumpAccel = 6;
	private float slamMeter;
	private float health;
	private bool down;
	private bool slamAllowed;
	private bool space;
	private bool spaceUp;
	private Animator animator;
	private static int lives = 1;
	private static float score;


	public const int STATE_SKATING = 0x00;
	public const int STATE_CROUCHING = 0x01;
	public const int STATE_JUMPING = 0x02;
	public const int STATE_BAILING = 0x03;
	public const int STATE_FLIPPING = 0x04;
	public const int STATE_SLAMMING = 0x05;
	
	// Use this for initialization
	void Start () {
		state = STATE_SKATING;
		animator = GetComponent<Animator> ();
		hud.UpdateLives(lives);
		gameValues.speed = speed;
		hud.UpdateHealth (1f);
		hud.UpdateSlam (1f);
		slamMeter = 1f;
		health = 100;
	}
	
	// Update is called once per frame
	void Update () {
		if (rigidbody2D.velocity.y >= maxVerticalVelocity) {
			rigidbody2D.velocity = new Vector3(rigidbody2D.velocity.x, maxVerticalVelocity);
		} 
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
			} else if(down && slamAllowed) {
				InitiateSlam();
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
		if (potentialJumpAccel < .5f * jumpAccel) {
			slamAllowed = false;
		} else if (slamMeter == 1.0f) {
			slamAllowed = true;
		}
		potentialJumpAccel = 6;
	}
	
	void UpdateScore() {
		score += (scoreMultiplier * -gameValues.speed * Time.deltaTime);
		hud.UpdateScore(score);
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (CheckForEnemyCollision (col)) {
			return;
		} else if (CheckForBouncyCollision (col)) {
			state = STATE_JUMPING;
			return;
		} else if (CheckForGroundCollision (col)) {
			Land ();
		}

		// STILL NEED TO HANDLE NON-ENEMY OBJECT COLLISIONS
	}

	bool CheckForEnemyCollision(Collision2D col) {
		Enemy enemy = col.gameObject.GetComponent<Enemy> ();
		if (!enemy) {
			return false;
		}
		TakeDamage (enemy.damage);
		return true;
	}

	bool CheckForBouncyCollision(Collision2D col) {
		return (col.collider.gameObject.GetComponent<BouncyBlocks> () != null);
	}

	bool CheckForGroundCollision(Collision2D col) {
		Vector2 contactNormal = col.contacts [0].normal;
		return contactNormal.normalized.y == 1.0;
	}

	/*bool CheckForCollidableObjectCollision(Collision2D col) {
		CollidableObject obj = 
	}*/


	void Land() {
		if(state == STATE_SLAMMING) {
			InvokeRepeating ("FillSlam", slamRegenTime, slamRegenTime);
		}
		state = STATE_SKATING;
		gameValues.SetSpeed (speed);
	}

	void TakeDamage(float damageToTake) {
		health -= damageToTake;
		print (health);
		hud.UpdateHealth (health / 100.0f);
		if (health <= 0) {
			Die ();
		}
		state = STATE_BAILING;
		animator.SetTrigger ("Bail");
		gameValues.SetSpeed(0);
	}

	void StandUp() {
		state = STATE_SKATING;
		gameValues.SetSpeed(speed);
	}
	
	public void SetYPos(float pos) {
		transform.position = new Vector3(transform.position.x, pos);
	}
	
	public void Die() {
		lives--;
		int deathState = GameLoopManager.DEATH_RELOAD;
		if(lives < 0) {
			deathState = GameLoopManager.DEATH_CONTINUE;
			lives = 3;
		} else {
			hud.UpdateLives(lives);
		}
		gameLoop.Die (deathState);
		Destroy (gameObject);
	}

	public void Bounce() {
		state = STATE_JUMPING;
	}

	void InitiateSlam() {
		gameValues.speed = 0;
		state = STATE_FLIPPING;
		rigidbody2D.gravityScale = 0;
		rigidbody2D.velocity = new Vector3 (0, 0);
		slamAllowed = false;
		slamMeter = 0;
		hud.UpdateSlam (slamMeter);
	}

	/*
	 * But but but wait it gets worse!
	 * I'm not watered down so I'm dying of thirst
	 * Coming through with the scam,
	 * Fool proof plan
	 * B-boys make some noise
	 * and just, JUST...
	 */

	void Slam() {
		rigidbody2D.velocity = Vector3.down * slamSpeed;
		state = STATE_SLAMMING;
		rigidbody2D.gravityScale = 1;
	}

	void FillSlam() {
		if (slamMeter>= 1) {
			CancelInvoke ();
			slamAllowed = true;
			slamMeter = 1.0f;
		} else {
			slamMeter += 1.0f / 14.0f;
			hud.UpdateSlam(slamMeter);
		}
	}
}
