using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animator))]
[RequireComponent (typeof (Skater))]
public class Skater : MonoBehaviour, IInputListener {

	public float initialJumpImpulse;
	public float maintainJump;
	public float maxJumpTime;
	public float skateSpeed;
	public float scoreMultiplier;
	public float maxVerticalVelocity;
	public float slamRegenTime;
	public float slamSpeed;
	public float infectionSpreadRate;
	public float flashDuration;
	public float fallDistanceThreshold;
	public float startXWide;
	public float startXNarrow;
	public GameValues gameValues;
	public GameLoopManager gameLoop;
	public GameObject spriteBody;
	public HUDManager hud;
	public AudioClip jumpClip;
	public AudioClip hitClip;
	public AudioClip landClip;
	public AudioClip flipClip;
	public AudioClip deathClip;
	public Song levelSong;

	private int state;
	private int direction;
	private float slamMeter;
	private float health;
	private float speed;
	private bool slamButton;
	private bool slamAllowed;
	private bool jumpButtonDown;
	private bool jumpButtonUp;
	private bool leftButton;
	private bool rightButton;
	private bool infected;
	private bool isInvincible;
	private bool fallingDetectedLast;
	private bool getUpOnLand;
	private bool dieOnGetUp;
	private bool onGround;
	private bool mirror;
	private bool noJump;
	private bool jumpOnNext;
	private Animator animator;
	public static int lives = 1;
	private static int coins;
	private static float score;
	private Color[] colors = new Color[] {new Color(.533f, .07843f, 0), new Color(1, 1, 1)};
	private int colorCounter = 0;
	private SpriteRenderer renderer;
	private float lastFlash;
	private float distanceToGround;
	private float totalJumpTime;
	private Vector3 pos;
	private Vector3 vel;
	private RaycastHit2D groundHit;
	private InputHandler handler;
	//private Quaternion facingLeft = Quaternion.Euler (0, 180, 0);
	//private Quaternion facingRight = Quaternion.Euler (0, 0, 0);
	private Vector3 facingLeft = new Vector3(-1, 1, 1);
	private Vector3 facingRight = new Vector3(1, 1, 1);

	public const int STATE_SKATING =   0x00;
	public const int STATE_CROUCHING = 0x01;
	public const int STATE_JUMPING =   0x02;
	public const int STATE_BAILING =   0x03;
	public const int STATE_FLIPPING =  0x04;
	public const int STATE_SLAMMING =  0x05;
	public const int STATE_FALLING =   0x06;
	public const int STATE_IDLE = 0x08;

	const int RIGHT = 1;
	const int LEFT = -1;

	// Use this for initialization
	void Start () {
		pos = transform.position;
		if (Camera.main.aspect <= 1.5) {
			pos.x = startXNarrow;
		} else {
			pos.x = startXWide;
		}
		transform.position = pos;
		SetState(STATE_IDLE);
		animator = GetComponent<Animator> ();
		hud.UpdateLives(lives);
		hud.UpdateCoins (coins);
		gameValues.speed = skateSpeed;
		hud.UpdateHealth (1f);
		hud.UpdateSlam (1f);
		slamMeter = 1f;
		health = 100;
		renderer = GetComponentInChildren<SpriteRenderer> ();
		SetupInputHandler ();
		direction = RIGHT;
		vel = transform.rigidbody2D.velocity;

	}

	void SetState(int newState) {
		state = newState;
	}

	void SetupInputHandler() {
		GameObject handlerParent = GameObject.Find ("InputHandler");
		if (!handlerParent) {
			handlerParent = new GameObject();
			handlerParent.name = "InputHandler";
			handlerParent.AddComponent("InputHandler");	
			handlerParent.transform.parent = transform.parent;
			
		} 
		handler = handlerParent.GetComponent<InputHandler>();
		handler.RegisterListener (this);
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
		vel = transform.rigidbody2D.velocity;
		speed = 0;
		HandleInput ();
		SetAnimState ();
		UpdateScore(scoreMultiplier * -gameValues.speed * Time.deltaTime);
		CheckInfection();
		if (isInvincible) {
			Flash ();
		}
		UpdateJumpVel ();
		CheckForFalling ();
		gameValues.SetSpeed (speed);
	}

	void UpdateJumpVel() {
		if (jumpOnNext) {
			Jump ();
		}
		transform.rigidbody2D.velocity = vel;
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
		if (rightButton || leftButton) {
			UpdateSpeedAndDirection(leftButton ? LEFT : RIGHT);
		}
		switch (state) {
		case STATE_SKATING:
		case STATE_IDLE:
			if(jumpButtonDown) {
				CrouchAndJump();
			} else if(leftButton || rightButton) {
				Skate ();
			} else {
				SetState (STATE_IDLE);
			}
			break;
		case STATE_JUMPING: 
			if(jumpButtonDown) {
				AddToJump();
			} else if(!noJump) {
				noJump = true;
			}
			break;
		case STATE_FALLING:
			if(slamButton && slamAllowed) {
				InitiateSlam();
			}
			break;
		case STATE_CROUCHING:
			break;

		}
	}

	void UpdateSpeedAndDirection(int newDirection) {
		if (newDirection == LEFT && direction == RIGHT) {
			//transform.localRotation = facingLeft;
			//transform.eulerAngles =  facingLeft;
			spriteBody.transform.localScale = facingLeft;
		} else if (newDirection == RIGHT && direction == LEFT) {
			//transform.localRotation = facingRight;
			//transform.eulerAngles = facingRight;
			spriteBody.transform.localScale = facingRight;
		}
		speed = skateSpeed * newDirection;
		direction = newDirection;
	}

	void Skate() {
		if(state != STATE_JUMPING) {
			SetState (STATE_SKATING);
		}
	}
	
	void CrouchAndJump() {
		SetState (STATE_CROUCHING);
		animator.SetTrigger ("Crouch");
		noJump = false;
	}

	public void SetJump() {
		jumpOnNext = true;
	}

	public void Jump() {
		SetState (STATE_JUMPING);
		vel.y = initialJumpImpulse;
		jumpOnNext = false;
	}

	void AddToJump() {
		if (noJump || jumpOnNext) {
			return;
		}
		if (totalJumpTime > maxJumpTime) {
			noJump = true;
		} else {
			vel.y += maintainJump;
		}
		totalJumpTime += Time.deltaTime;
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
		if (!infected) {
			return;
		}
		UpdateHealth (health - (infectionSpreadRate * Time.deltaTime));
		if (health < 25) {
			SetAnimLayer(3);
		} else if (health < 50) {
			SetAnimLayer(2);
		} else {
			SetAnimLayer(1);
		}
	}

	void SetAnimLayer(int layer) {
		for (int i = 0; i < 4; i++) {
			animator.SetLayerWeight(i, 0);
		}
		animator.SetLayerWeight (layer, 1);
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
		totalJumpTime = 0;
		noJump = false;
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
		SetState(STATE_IDLE);
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

	public void OnInput(bool[] inputs) {
		jumpButtonUp = jumpButtonDown && !inputs [1];
		this.slamButton = inputs[0];
		this.jumpButtonDown = inputs[1];
		this.leftButton = inputs [6];
		this.rightButton = inputs [7];
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
		handler.UnregisterListener (this);
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
