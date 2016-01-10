using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {

	public Background background;
	public Foreground foreground;
	public Skater skater;

	bool down;
	bool left;
	bool right;
	bool space;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		down = Input.GetKey (KeyCode.DownArrow);
		left = Input.GetKey (KeyCode.LeftArrow);
		right = Input.GetKey (KeyCode.RightArrow);
		space = Input.GetKey (KeyCode.Space);
		background.SetInput(left, right);
		foreground.SetInput(left, right);
		skater.SetInput (down, left, right, space);
	}
}
