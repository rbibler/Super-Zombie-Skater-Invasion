using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animator))]
public class InvincibilityPowerUp : MonoBehaviour {

	public float invincibilityDuration;

	private Animator animator;


	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	void OnTriggerEnter2D(Collider2D col) {
		Skater skater = col.gameObject.GetComponent<Skater> ();
		if (!skater) {
			return;
		}
		skater.SetInvincibility (invincibilityDuration);
		animator.SetTrigger ("collected");
	}

	void DestroyMe() {
		Destroy (gameObject);
	}
}
