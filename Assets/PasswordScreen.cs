using UnityEngine;
using System.Collections;

public class PasswordScreen : MonoBehaviour {

	public int charsWide;
	public int charsHigh;
	public GameObject cursor;
	public Vector2 startPos;
	
	private Vector2 currentPos;
	private Vector2 gridPos = new Vector2(0, 0);

	// Use this for initialization
	void Start () {
		currentPos = new Vector2(startPos.x, startPos.y);
	}
	
	// Update is called once per frame
	void Update () {
		CheckInput();
	}
	
	void CheckInput() {
		if(Input.GetKeyUp (KeyCode.DownArrow)) {
			MoveCursorVertical(-2);
		} else if(Input.GetKeyUp(KeyCode.UpArrow)) {
			MoveCursorVertical(2);
		} else if(Input.GetKeyUp (KeyCode.LeftArrow)) {
			MoveCursorHoriz(-2);
		} else if(Input.GetKeyUp (KeyCode.RightArrow)) {
			MoveCursorHoriz(2);
		}
		cursor.transform.position = currentPos;
		CalculateGridPos();
	}
	
	void CalculateGridPos() {
		gridPos.x = (currentPos.x - startPos.x) / 2;
		gridPos.y = (currentPos.y - startPos.y) / -2;
		print (gridPos);
	}
	
	void MoveCursorVertical(int direction) {
		currentPos.y += direction;
		if(currentPos.y < (startPos.y - charsHigh) && direction < 0) {
			currentPos.y = startPos.y;
		} else if(currentPos.y > startPos.y && direction > 0) {
			currentPos.y = startPos.y - charsHigh;
		}
	}
	
	void MoveCursorHoriz(int direction) {
		currentPos.x += direction;
		if(currentPos.x > startPos.x + charsWide) {
			currentPos.x = startPos.x;
		} else if(currentPos.x < startPos.x) {
			currentPos.x = startPos.x + charsWide;
		}
		
	}
}
