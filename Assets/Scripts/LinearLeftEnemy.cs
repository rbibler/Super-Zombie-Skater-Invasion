using UnityEngine;
using System.Collections;

public class LinearLeftEnemy : MonoBehaviour {

	public float xVel;
	public GameValues values;

	private Vector3 pos;
	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		pos = transform.position;
		spriteRenderer = GetComponentInChildren<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		pos.y = transform.position.y;
		float posToMove = values.speed;
		if (spriteRenderer.isVisible) {
			posToMove += xVel;
		}
		pos.x += posToMove * Time.deltaTime;
		transform.position = pos;
	}


	
}
