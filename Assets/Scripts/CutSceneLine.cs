using UnityEngine;
using System.Collections;

public class CutSceneLine : MonoBehaviour {

	public string lineText;
	public Vector2 startPoint;

	private char[] text;
	private int currentLetterPos;

	// Use this for initialization
	void Start () {
		text = lineText.ToCharArray ();
	}

	public bool DrawNextChar(Alphabet alphabet, Letter letter, Transform cutSceneParent) {
		char currentLetter = text[currentLetterPos];
		Sprite s = alphabet.GetLetter(currentLetter);
		Letter newLetter = Instantiate (letter) as Letter;
		newLetter.transform.parent = cutSceneParent;
		newLetter.GetComponent<SpriteRenderer>().sprite = s;
		newLetter.transform.position = new Vector3(startPoint.x, startPoint.y, -1);
		startPoint.x += 1;
		currentLetterPos++;
		if(currentLetterPos >= text.Length) {
			return true;
		}
		return false;
	}
	

}
