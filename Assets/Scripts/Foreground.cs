using UnityEngine;
using System.Collections;

public class Foreground : MonoBehaviour {

	private Vector3 pos;
	
	public GameValues gameValues;
	// Use this for initialization
	void Start () {
		pos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		pos.x += gameValues.speed * Time.deltaTime;
		transform.position = pos;
	}
	
}
