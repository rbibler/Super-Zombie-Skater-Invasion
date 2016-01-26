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
	public float flashDuration;
	public float fallDistanceThreshold;
	public GameValues gameValues;
	public GameLoopManager gameLoop;
	public HUDManager hud;
	public AudioClip jumpClip;
	public AudioClip hitClip;
	public AudioClip landClip;
	public AudioClip flipClip;
	public AudioClip deathClip;
	public Song levelSong;
	[Range (0, 0x3F)]
	public int colorToRender;

	private int state;
	private int lastState;
	private float potentialJumpAccel = 6;
	private float slamMeter;
	private float health;
	private bool down;
	private bool slamAllowed;
	private bool space;
	private bool spaceUp;
	private bool infected;
	private bool isInvincible;
	private bool fallingDetectedLast;
	private bool getUpOnLand;
	private bool dieOnGetUp;
	private bool onGround;
	private Animator animator;
	private static int lives = 1;
	private static int coins;
	private static float score;
	private Color[] colors = new Color[] {new Color(.533f, .07843f, 0), new Color(1, 1, 1)};
	private int[] infectionColors = new int[] {60, 42, 34, 0x3C, 24, 0x2D, 0x10, 0x20 ,0x3D, 0x30};
	private int colorCounter = 0;
	private SpriteRenderer renderer;
	private float lastFlash;
	private float lastVel;
	private float distanceToGround;
	private Vector3 pos;
	private RaycastHit2D groundHit;
	public Color skinColor;

	public const int STATE_SKATING = 0x00;
	public const int STATE_CROUCHING = 0x01;
	public const int STATE_JUMPING = 0x02;
	public const int STATE_BAILING = 0x03;
	public const int STATE_FLIPPING = 0x04;
	public const int STATE_SLAMMING = 0x05;
	public const int STATE_FALLING = 0x06;
	
	// Use this for initialization
	void Start () {
		SetState(STATE_SKATING);
		animator = GetComponent<Animator> ();
		hud.UpdateLives(lives);
		hud.UpdateCoins (coins);
		gameValues.speed = speed;
		hud.UpdateHealth (1f);
		hud.UpdateSlam (1f);
		slamMeter = 1f;
		health = 100;
		renderer = GetComponentInChildren<SpriteRenderer> ();
		pos = transform.position;
	}

	void SetState(int newState) {
		state = newState;
	}

	bool GetHitDistance(out float distance) {
		distance = 0f;
		groundHit = Physics2D.Raycast (transform.position, -Vector2.up);
		onGround = false;
		if (groundHit.collider != null) {
			distance = groundHit.distance;
			if(distance == 0) {
				onGround = true;
			} 
			return true;
		}
		return false;
	}
	
	// Update is called once per frame
	void Update () {
		pos.y = transform.position.y;
		transform.position = pos;
		if (rigidbody2D.velocity.y >= maxVerticalVelocity) {
			rigidbody2D.velocity = new Vector3(rigidbody2D.velocity.x, maxVerticalVelocity);
		} 
		HandleInput ();
		SetAnimState ();
		UpdateScore(scoreMultiplier * -gameValues.speed * Time.deltaTime);
		CheckInfection();
		if (isInvincible) {
			Flash ();
		}
		lastVel = rigidbody2D.velocity.y;
		CheckForFalling ();
	}

	void CheckForFalling() {
		if ((!GetHitDistance (out distanceToGround) || distanceToGround > fallDistanceThreshold)) {
			fallingDetectedLast = true;
			if (fallingDetectedLast && rigidbody2D.velocity.y < 0 && state == STATE_SKATING) {
				SetState (STATE_FALLING);
			}
		} else {
			fallingDetectedLast = false;
		}
	}

	void Flash() {
		if (Time.timeSinceLevelLoad - lastFlash > flashDuration) {
			lastFlash = Time.timeSinceLevelLoad;
			renderer.color = colors [colorCounter++];
			if (colorCounter >= colors.Length) {
				colorCounter = 0;
			}
		}
	}

	void SetAnimState() {
		animator.SetInteger ("state", state);
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
		case STATE_FALLING:
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
			SetState(STATE_CROUCHING);
		}
	}
	
	void Jump() {
		SetState(STATE_JUMPING);
		transform.rigidbody2D.velocity = Vector3.up * potentialJumpAccel;
		if (potentialJumpAccel < .5f * jumpAccel) {
			slamAllowed = false;
		} else if (slamMeter == 1.0f) {
			slamAllowed = true;
		}
		potentialJumpAccel = 6;
		AudioSource.PlayClipAtPoint (jumpClip, transform.position);
	}

	void InitiateSlam() {
		gameValues.speed = 0;
		SetState(STATE_FLIPPING);
		rigidbody2D.gravityScale = 0;
		rigidbody2D.velocity = new Vector3 (0, 0);
		slamAllowed = false;
		slamMeter = 0;
		hud.UpdateSlam (slamMeter);
		AudioSource.PlayClipAtPoint (flipClip, transform.position);
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
		SetState(STATE_SLAMMING);
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
	
	void CheckInfection() {
		if(infected) {
			UpdateHealth(health - (infectionSpreadRate * Time.deltaTime));
			int colorToGet = (int) (health / 10);
			if(renderer.sprite == null) {
				return;
			}
			Texture2D texture = renderer.sprite.texture;
			Color[] colors = renderer.sprite.texture.GetPixels ();
			Color newColor = NESColorPalette.GetColor (infectionColors[colorToGet]);
			if(colors != null) {
				Color color;
				for(int i = 0; i < colors.Length; i++) {
					color = colors[i];
					if(color == skinColor) {
						colors[i] = newColor;
					}
				}
				texture.SetPixels(colors);
				texture.Apply();
			}
			skinColor = newColor;
			//renderer.color = NESColorPalette.GetColor(infectionColors[colorToGet]);
		}
	}

	void UpdateColors() {

	}
	
	void UpdateHealth(float health) {
		this.health = health;
		if (health >= 100) {
			health = 100;
		}
		hud.UpdateHealth(health / 100.0f);
		if(health <= 0) {
			infected = false;
			levelSong.Stop();
			AudioSource.PlayClipAtPoint(deathClip, transform.position);
			dieOnGetUp = true;
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		// Always check to see if we've landed
		CheckForGroundCollision (col);
		CheckForBouncyCollision (col);
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
		if(state == STATE_SLAMMING || isInvincible) {
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
			SetState(STATE_JUMPING);
		}
	}

	void CheckForGroundCollision(Collision2D col) {
		if (col.collider.gameObject.tag != "Ground") {
			return;
		}
		Vector2 contactNormal = col.contacts [0].normal;
		if (contactNormal.normalized.y == 1.0) {
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
	
	void Land() {
		if(state == STATE_BAILING) {
			if(dieOnGetUp) {
				Invoke ("Die", 2.0f);
				return;
			}
			if(getUpOnLand) {
				getUpOnLand = false;
				animator.SetTrigger("Get Up");
			}
			return;
		}
		if(state == STATE_SLAMMING) {
			AudioSource.PlayClipAtPoint(landClip, transform.position);
			InvokeRepeating ("FillSlam", slamRegenTime, slamRegenTime);
		}
		SetState(STATE_SKATING);
		gameValues.SetSpeed (speed);
	}

	void TakeDamage(float damageToTake) {
		if (isInvincible || state == STATE_BAILING) {
			return;
		}
		SetState(STATE_BAILING);
		animator.SetTrigger ("Bail");
		gameValues.SetSpeed(0);
		UpdateHealth (health - damageToTake);
		AudioSource.PlayClipAtPoint (hitClip, transform.position);
	}

	void StandUp() {
		SetState(STATE_SKATING);
		gameValues.SetSpeed(speed);
	}

	void MakeMortal() {
		isInvincible = false;
		renderer.color = Color.white;
	}

	public void BailOver() {
		if (onGround && health > 0) {
			animator.SetTrigger("Get Up");
		} else {
			getUpOnLand = true;
		}
	}

	public void SetInput(bool down, bool space, bool spaceUp) {
		this.down = down;
		this.space = space;
		this.spaceUp = spaceUp;
	}
	
	public void SetYPos(float pos) {
		transform.position = new Vector3(transform.position.x, pos);
	}

	public void UpdateScore(float scoreToAdd) {
		score += scoreToAdd;
		hud.UpdateScore(score);
	}

	public void FellIntoGap() {
		levelSong.Stop();
		AudioSource.PlayClipAtPoint(deathClip, transform.position);
		gameValues.speed = 0;
		Invoke ("Die", 2.0f);
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

	public void SetInvincibility(float invincibilityDuration) {
		isInvincible = true;
		Invoke ("MakeMortal", invincibilityDuration);
	}
}
