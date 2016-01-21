using UnityEngine;
using System.Collections;
[RequireComponent (typeof (Animator))]
public class Enemy : MonoBehaviour {

	public float damage;
	public float bonusForKilling;
	public bool isZombie;
	public bool isDangerous = true;
	
	private Animator animator;
	
	void Start() {
		animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () {
	
	}
	
	public void Die() {
		animator.SetTrigger("Die");
		isDangerous = false;
	}
	
	void DestroyMe() {
		Destroy (gameObject);
	}
}
