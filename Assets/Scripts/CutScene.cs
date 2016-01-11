using UnityEngine;
using System.Collections;

public class CutScene : MonoBehaviour {

	public Alphabet alphabet;
	public Letter letter;
	public string textToDraw;
	public float timeDelay;
	public Vector2 startPoint;

	private float lastDrawTime;
	private int currentLetterPos;
	private char[] text;

	// Use this for initialization
	void Start () {
		text = textToDraw.ToCharArray ();
	}
	
	// Update is called once per frame
	void Update () {
		float timeSinceLast = Time.timeSinceLevelLoad - lastDrawTime;
		if(timeSinceLast >= timeDelay) {
			char currentLetter = text[currentLetterPos];
			Sprite s = alphabet.GetLetter(currentLetter);
			Letter newLetter = Instantiate (letter) as Letter;
			newLetter.transform.parent = transform;
			newLetter.GetComponent<SpriteRenderer>().sprite = s;
			newLetter.transform.position = new Vector3(startPoint.x, startPoint.y, -1);
			startPoint.x += 1;
			currentLetterPos++;
			if(currentLetterPos >= text.Length) {
				currentLetterPos = 0;
			}
			lastDrawTime = Time.timeSinceLevelLoad;
		}
	}
}
