using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animator))]
public class BouncyBlocks : MonoBehaviour {
	
	private Animator animator;


	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}

	void OnCollisionEnter2D(Collision2D col) {
		animator.SetTrigger ("Bounce");
	} 
	
}
