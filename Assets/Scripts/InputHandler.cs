using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {

	public Skater skater;

	bool down;
	bool left;
	bool right;
	bool space;
	bool spaceUp;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		down = Input.GetKey (KeyCode.DownArrow);
		space = Input.GetKey (KeyCode.Space);
		spaceUp = Input.GetKeyUp (KeyCode.Space);
		skater.SetInput (down, space, spaceUp);
	}
}
