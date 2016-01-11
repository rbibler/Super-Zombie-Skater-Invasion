using UnityEngine;
using System.Collections;

public class LevelSplash : MonoBehaviour, IListener {

	public float splashTime;
	public string levelToLoad;
	public AnimatedFader fader;
	public LevelManager manager;

	private bool fadeStarted;
	
	void Start() {
		fader.RegisterListener(this);
	}

	
	
	// Update is called once per frame
	void Update () {
		if(Time.timeSinceLevelLoad >= splashTime && !fadeStarted) {
			fadeStarted = true;
			fader.StartFadeAnimation(AnimatedFader.FADE_OUT);
		}
	}
	
	void IListener.OnNotification(string message) {
		if(message.Contains ("Fader Finished")) {
			message = message.Substring (message.IndexOf(":") + 1);
			print (message);
			int faderState = 0;
			int.TryParse(message, out faderState);
			if(faderState == 5) {
				manager.LoadLevelByName(levelToLoad);
			}
		}
	}
}
