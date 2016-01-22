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
	public float infectionSpreadRate;
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
	private bool infected;
	private Animator animator;
	private static int lives = 1;
	private static int coins;
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
		hud.UpdateCoins (coins);
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
		UpdateScore(scoreMultiplier * -gameValues.speed * Time.deltaTime);
		CheckInfection();
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
	
	public void UpdateScore(float scoreToAdd) {
		score += scoreToAdd;
		hud.UpdateScore(score);
	}
	
	void CheckInfection() {
		if(infected) {
			UpdateHealth(health - (infectionSpreadRate * Time.deltaTime));
		}
	}
	
	void UpdateHealth(float health) {
		this.health = health;
		if (health >= 100) {
			health = 100;
		}
		hud.UpdateHealth(health / 100.0f);
		if(health <= 0) {
			Die ();
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		// Always check to see if we've landed
		CheckForGroundCollision (col);
		CheckForBouncyCollision (col);
		CheckForPowerupCollision(col);
		if (CheckForEnemyCollision (col)) {
			return;
		} else {
			CheckForBGObjectCollision(col);
		} 
	}

	bool CheckForEnemyCollision(Collision2D col) {
		Enemy enemy = col.gameObject.GetComponent<Enemy> ();
		if (!enemy) {
			return false;
		}
		if(state == STATE_SLAMMING) {
			enemy.Die ();
			UpdateScore (enemy.bonusForKilling);
			return false;
		} else if(enemy.isDangerous) {
			TakeDamage (enemy.damage);
			if(enemy.isZombie) {
				infected = true;
			}
		}
		return true;
	}

	void CheckForBouncyCollision(Collision2D col) {
		BouncyBlocks bouncy = col.collider.gameObject.GetComponent<BouncyBlocks>();
		if(bouncy != null) {
			state = STATE_JUMPING;
		}
	}

	void CheckForGroundCollision(Collision2D col) {
		Vector2 contactNormal = col.contacts [0].normal;
		if(contactNormal.normalized.y == 1.0) {
			Land ();
		}
	}

	void CheckForBGObjectCollision(Collision2D col) {
		CollidableObject collidableObj = col.gameObject.GetComponent<CollidableObject>();
		Vector2 contactNormal = col.contacts[0].normal;
		if(!collidableObj) {
			return;
		} 
		if(contactNormal.normalized.y == -1.0 || contactNormal.normalized.x == -1.0) {
			TakeDamage (collidableObj.damage);
		}
	}
	
	void CheckForPowerupCollision(Collision2D col) {
	
	}


	void Land() {
		if(state == STATE_BAILING) {
			return;
		}
		if(state == STATE_SLAMMING) {
			InvokeRepeating ("FillSlam", slamRegenTime, slamRegenTime);
		}
		state = STATE_SKATING;
		gameValues.SetSpeed (speed);
	}

	void TakeDamage(float damageToTake) {
		state = STATE_BAILING;
		animator.SetTrigger ("Bail");
		gameValues.SetSpeed(0);
		UpdateHealth (health - damageToTake);
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

	public void UpdateLives(int livesToAdd) {
		lives += livesToAdd;
		hud.UpdateLives (lives);
	} 

	public void AddCoin(float scoreToAdd) {
		UpdateScore (scoreToAdd);
		coins++;
		if (coins == 100) {
			UpdateLives(1);
			coins = 0;
		}
		hud.UpdateCoins (coins);
	}

	public void UpdateHealthFromPowerUp(float healthToIncrease, float scoreToAdd) {
		infected = false;
		UpdateHealth (health + healthToIncrease);
		UpdateScore (scoreToAdd);
	}
}
