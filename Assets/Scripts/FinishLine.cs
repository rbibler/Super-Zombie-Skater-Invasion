using UnityEngine;
using System.Collections;

public class FinishLine : MonoBehaviour {

	public string levelToLoad;
	public LevelManager manager;
	public AnimatedFader fader;

	void OnTriggerEnter2D(Collider2D col) {
		Skater skater = col.gameObject.GetComponent<Skater> ();
		if (!skater) {
			return;
		}
		fader.StartFadeAnimation(AnimatedFader.FADE_OUT);
		Invoke ("LoadNextLevel", 1.5f);
	}

	void LoadNextLevel() {
		manager.LoadLevelByName (levelToLoad);
	}

		                      
}
