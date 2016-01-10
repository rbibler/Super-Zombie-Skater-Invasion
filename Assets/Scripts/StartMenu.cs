using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour {
	
	public AnimatedFader fader;

	// Use this for initialization
	void Awake () {
		if(LevelManager.previousLevel == "01b Options") {
			fader.startState = AnimatedFader.SLIDE_OUT_LEFT;
		} else {
			fader.startState = AnimatedFader.FADE_IN;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
