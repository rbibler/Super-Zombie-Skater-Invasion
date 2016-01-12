using UnityEngine;
using System.Collections;

public class CutSceneCard : MonoBehaviour {
	
	public float timeDelay;
	public float persistTime;
	public bool startFullyShown;
	public CutSceneLine[] linesInCard;

	private float lastDrawTime;
	private CutSceneLine currentLine;
	private int currentLineIndex;
	private bool persistState;


	void Start() {

		currentLine = linesInCard [0];
		lastDrawTime = Time.timeSinceLevelLoad;
	}

	public bool DrawLine(Alphabet alphabet, Letter letter) {
		if (startFullyShown && !persistState) {
			ShowFullLine(alphabet, letter);
		}
		float timeSinceLast = Time.timeSinceLevelLoad - lastDrawTime;
		if (persistState) {
			if(timeSinceLast >= persistTime) {
				return true;
			}
			return false;
		}

		if(timeSinceLast >= timeDelay) {
			lastDrawTime = Time.timeSinceLevelLoad;
			ShowLine (alphabet, letter);
		}
		return false;
	}

	bool ShowLine(Alphabet alphabet, Letter letter) {
		if (currentLine.DrawNextChar (alphabet, letter, transform)) {
			currentLineIndex++;
			if (currentLineIndex >= linesInCard.Length) {
				persistState = true;
				return false;
			}
			currentLine = linesInCard [currentLineIndex];
		}
		return true;
	}

	void ShowFullLine(Alphabet alphabet, Letter letter) {
		bool moreLines = true;
		while (moreLines) {
			moreLines = ShowLine (alphabet, letter);
		}
	}
}
