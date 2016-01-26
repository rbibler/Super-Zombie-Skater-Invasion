using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animator))]
public class OneUpPowerUP : MonoBehaviour {

	public float scoreToAdd;

	private Animator animator;

	void Start() {
		animator = GetComponent<Animator> ();
	}

	void OnTriggerEnter2D(Collider2D col) {
		Skater skater = col.gameObject.GetComponent<Skater>();
		if(!skater) {
			return;
		}
		skater.UpdateScore(scoreToAdd);
		skater.UpdateLives(1);
		animator.SetTrigger ("collected");
	}
}
