using UnityEngine;
using System.Collections;
using System.Text;

public class PasswordScreen : MonoBehaviour, IInputListener {

	public int charsWide;
	public int charsHigh;
	public GameObject cursor;
	public GameObject hangmanCursor;
	public Vector2 startPos;
	public Letter[] hangmanLetters;
	public Alphabet alphabet;
	public PasswordManager manager;
	public Sprite spaceSprite;
	public GameObject successMessage;
	public GameObject failureMessage;
	public GameObject hangman;
	
	private Vector2 currentPos;
	private Vector2 gridPos = new Vector2(0, 0);
	private int currentHangmanPos;
	private char[] password = "                ".ToCharArray();
	private InputHandler handler;
	private float lastVeritcalMove;
	private float lastHorizontalMove;
	private float lastEnter;
	private float delayAllowed = .25f;

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
		SetupInputHandler ();
		lastVeritcalMove = lastHorizontalMove = lastEnter = Time.timeSinceLevelLoad;
	}

	void SetupInputHandler() {
		GameObject handlerParent = GameObject.Find ("InputHandler");
		if (!handlerParent) {
			handlerParent = new GameObject();
			handlerParent.name = "InputHandler";
			handlerParent.AddComponent("InputHandler");	
			handlerParent.transform.parent = transform.parent;
			
		} 
		handler = handlerParent.GetComponent<InputHandler>();
		handler.RegisterListener (this);
	}

	public void OnInput(bool[] inputs) {
		if (inputs[5]) {
			MoveCursorVertical (-2);
		} else if (inputs[4]) {
			MoveCursorVertical (2);
		} else if (inputs[6]) {
			MoveCursorHoriz (-2);
		} else if (inputs[7]) {
			MoveCursorHoriz (2);
		} else if (inputs[1]) {
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
		if (Time.timeSinceLevelLoad - lastVeritcalMove < delayAllowed) {
			return;
		}
		lastVeritcalMove = Time.timeSinceLevelLoad;
		currentPos.y += direction;
		if(currentPos.y < (startPos.y - charsHigh) && direction < 0) {
			currentPos.y = startPos.y;
		} else if(currentPos.y > startPos.y && direction > 0) {
			currentPos.y = startPos.y - charsHigh;
		}
	}
	
	void MoveCursorHoriz(int direction) {
		if (Time.timeSinceLevelLoad - lastHorizontalMove < delayAllowed) {
			return;
		}
		lastHorizontalMove = Time.timeSinceLevelLoad;
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
		if (Time.timeSinceLevelLoad - lastEnter < delayAllowed) {
			return;
		}
		lastEnter = Time.timeSinceLevelLoad;
		char c = GetPasswordInput ();
		if (c == '!') {
			HangmanBack ();
		} else if (c == '#') {
			HangmanNext ();
		} else if (c == '%') {
			Delete ();
		} else if (c == '*') {
			if(manager.CheckPassword(CreateOutput())) {
				DisplayMessage(successMessage);
				handler.UnregisterListener(this);
			} else {
				DisplayMessage(failureMessage);
				Invoke ("ClearMessage", 2.0f);
			}
		} else {
			HandleAlphabeticEntry();
		}
	}

	void DisplayMessage(GameObject message) {
		hangman.SetActive (false);
		message.SetActive (true);
		hangmanCursor.SetActive (false);
	}

	void ClearMessage() {
		hangman.SetActive (true);
		hangmanCursor.SetActive (true);
		failureMessage.SetActive (false);
		foreach (Letter letter in hangmanLetters) {
			letter.GetComponentInChildren<SpriteRenderer>().sprite = spaceSprite;
		}
		currentHangmanPos = 0;
		UpdateHangmanCursor ();
	}

	void UpdateHangmanCursor() {
		hangmanCursor.transform.position = hangmanLetters [currentHangmanPos].transform.position;
	}

	char GetPasswordInput() {
			return passwordInputArray [(int)gridPos.y, (int)gridPos.x];
	}

	void Delete() {
		SetHangmanLetter (' ');
	}

	void HangmanBack() {
		currentHangmanPos -= 1;
		if (currentHangmanPos < 0) {
			currentHangmanPos = hangmanLetters.Length - 1;
		}
		UpdateHangmanCursor ();
	}

	void HangmanNext() {
		currentHangmanPos += 1;
		if (currentHangmanPos >= hangmanLetters.Length) {
			currentHangmanPos = 0;
		}
		UpdateHangmanCursor ();
	}

	void HandleAlphabeticEntry() {
		char c = passwordInputArray [(int)gridPos.y, (int)gridPos.x];
		SetHangmanLetter (c);
		HangmanNext ();
	}

	void SetHangmanLetter(char charToSet) {
		Sprite s = null;
		if (charToSet == ' ') {
			s = spaceSprite;
		} else {
			s = alphabet.GetLetter (charToSet);
		}
		if (s == null) {
			return;
		}
		hangmanLetters[currentHangmanPos].GetComponentInChildren<SpriteRenderer> ().sprite = s;
		password [currentHangmanPos] = charToSet;
	}

	string CreateOutput() {
		var builder = new StringBuilder ();
		foreach (char c in password) {
			builder.Append(c);
		}
		print (builder.ToString ());
		return builder.ToString ().TrimEnd();
	}
}
