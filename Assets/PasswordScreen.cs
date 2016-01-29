using UnityEngine;
using System.Collections;
using System.Text;

public class PasswordScreen : MonoBehaviour {

	public int charsWide;
	public int charsHigh;
	public GameObject cursor;
	public GameObject hangmanCursor;
	public Vector2 startPos;
	public Letter[] hangmanLetters;
	public Alphabet alphabet;
	
	private Vector2 currentPos;
	private Vector2 gridPos = new Vector2(0, 0);
	private int currentHangmanPos;
	private char[] password = "                ".ToCharArray();

	private char[,] passwordInputArray = new char[,] {
		{'A', 'a', 'B', 'b', 'C', 'c', 'D', 'd', 'E', 'e', 'Z', 'z'},
		{'F', 'f', 'G', 'g', 'H', 'h', 'I', 'i', 'J', 'j', '\'', '.'},
		{'K', 'k', 'L', 'l', 'M', 'm', 'N', 'n', 'O', 'o', '*', '&'},
		{'P', 'p', 'Q', 'q', 'R', 'r', 'S', 's', 'T', 't', '!', '@'}, 
		{'U', 'u', 'V', 'v', 'W', 'w', 'X', 'x', 'Y', 'y', '#', '$'},
		{'1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '%', '^'}
	};

	// Use this for initialization
	void Start () {
		currentPos = new Vector2(startPos.x, startPos.y);
	}
	
	// Update is called once per frame
	void Update () {
		CheckInput();
	}
	
	void CheckInput() {
		if (Input.GetKeyUp (KeyCode.DownArrow)) {
			MoveCursorVertical (-2);
		} else if (Input.GetKeyUp (KeyCode.UpArrow)) {
			MoveCursorVertical (2);
		} else if (Input.GetKeyUp (KeyCode.LeftArrow)) {
			MoveCursorHoriz (-2);
		} else if (Input.GetKeyUp (KeyCode.RightArrow)) {
			MoveCursorHoriz (2);
		} else if (Input.GetKeyUp (KeyCode.Return)) {
			CheckInputConfirm();
		}
		cursor.transform.position = currentPos;
		CalculateGridPos();
	}

	void CalculateGridPos() {
		gridPos.x = (int) (((currentPos.x - startPos.x) / 2) + .5);
		gridPos.y = (int) (((currentPos.y - startPos.y) / -2) + .5);
		if (gridPos.y >= 2) {
			if(gridPos.x >= 10) {
				gridPos.x = 10;
				currentPos.x = startPos.x + 20;
			}
		}
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
		print (gridPos);
		if(currentPos.x > (startPos.x + charsWide) || (gridPos.x >= 10 && direction > 0 && gridPos.y >= 2)) {
			currentPos.x = startPos.x;
		} else if(currentPos.x < startPos.x) {
			if(gridPos.y >= 2) {
				currentPos.x = startPos.x + 20;
			} else {
				currentPos.x = startPos.x + charsWide;
			}
		}
	}

	void CheckInputConfirm() {
		char c = GetPasswordInput ();
		if (c == '!') {
			HangmanBack ();
		} else if (c == '#') {
			HangmanNext ();
		} else if (c == '%') {
			Delete ();
		} else if (c == '*') {
			CreateOutput();
		} else {
			HandleAlphabeticEntry();
		}
	}
	char GetPasswordInput() {
			return passwordInputArray [(int)gridPos.y, (int)gridPos.x];
	}

	void Delete() {
		SetHangmanLetter ('_');
	}

	void HangmanBack() {
		currentHangmanPos -= 1;
		if (currentHangmanPos < 0) {
			currentHangmanPos = hangmanLetters.Length - 1;
		}
		hangmanCursor.transform.position = hangmanLetters [currentHangmanPos].transform.position;
	}

	void HangmanNext() {
		currentHangmanPos += 1;
		if (currentHangmanPos >= hangmanLetters.Length) {
			currentHangmanPos = 0;
		}
		hangmanCursor.transform.position = hangmanLetters [currentHangmanPos].transform.position;
	}

	void HandleAlphabeticEntry() {
		char c = passwordInputArray [(int)gridPos.y, (int)gridPos.x];
		SetHangmanLetter (c);
		HangmanNext ();
	}

	void SetHangmanLetter(char charToSet) {
		Sprite s = alphabet.GetLetter (charToSet);
		if (s == null) {
			return;
		}
		hangmanLetters[currentHangmanPos].GetComponentsInChildren<SpriteRenderer> ()[0].sprite = s;
		password [currentHangmanPos] = charToSet;
	}

	string CreateOutput() {
		var builder = new StringBuilder ();
		foreach (char c in password) {
			builder.Append(c);
		}
		print (builder.ToString ());
		return builder.ToString ();
	}
}
