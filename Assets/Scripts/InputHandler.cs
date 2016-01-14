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
		down = Input.GetKeyDown (KeyCode.DownArrow);
		space = Input.GetKey (KeyCode.Space) || (Input.GetMouseButton(0) && Input.mousePosition.x > Screen.width / 2);
		spaceUp = Input.GetKeyUp (KeyCode.Space) || (Input.GetMouseButtonUp (0) && Input.mousePosition.x > Screen.width / 2);
		skater.SetInput (down, space, spaceUp);
	}
	
	/*void OnMouseDown() {
		
		float xPos = Input.mousePosition.x;
		print ("Mouse Down");
		if(xPos > Screen.width / 2) {
			space = true;
		}
		skater.SetInput (false, space, false);
	}
	
	void OnMouseUp() {
		print ("Mouse Up");
		if(space) {
			space = false;
			spaceUp = true;
			skater.SetInput (false, false, spaceUp);
		}
	}*/
}
