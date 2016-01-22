using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PowerUp))]
[RequireComponent (typeof (Animator))]
public class Coin : MonoBehaviour {

	public float scoreIncrease;

	private Animator animator;
	void Start() {
		animator = GetComponent<Animator> ();
	}

	void OnTriggerEnter2D(Collider2D col) {
		Skater skater = col.gameObject.GetComponent<Skater> ();
		if (!skater) {
			return;
		}
		skater.AddCoin (scoreIncrease);
		animator.SetTrigger ("collected");
	}

	private void DestroyMe() {
		Destroy (gameObject);
	}
}
