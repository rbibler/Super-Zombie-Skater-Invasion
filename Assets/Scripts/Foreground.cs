using UnityEngine;
using System.Collections;

public class Foreground : MonoBehaviour {


	private bool left;
	private bool right;
	private Vector3 pos;
	
	public GameValues gameValues;
	
	// Use this for initialization
	void Start () {
		pos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		HandleInput();
		transform.position = pos;
	}
	
	void HandleInput() {
		if(left) {
			pos.x += gameValues.speed;
		} else if(right) {
			pos.x -= gameValues.speed;
		}
		
	}
	
	public void SetInput(bool left, bool right) {
		this.left = left;
		this.right = right;
	}
}
