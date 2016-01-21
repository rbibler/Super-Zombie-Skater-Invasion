using UnityEngine;
using System.Collections;

public class LinearLeftEnemy : MonoBehaviour {

	public float xVel;
	public GameValues values;

	private Vector3 pos;

	// Use this for initialization
	void Start () {
		pos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		pos.x += (values.speed + xVel) * Time.deltaTime;
		transform.position = pos;
	}
}
