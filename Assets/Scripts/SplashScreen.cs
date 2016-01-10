using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {
	
	public float splashDuration;
	public AnimatedFader fader;
	private LevelManager manager;
	private bool fadeSet;
	// Use this for initialization
	void Start () {
		manager = GameObject.FindObjectOfType<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.timeSinceLevelLoad >= splashDuration - 1 && !fadeSet) {
			fader.StartFadeAnimation(AnimatedFader.FADE_OUT);
			fadeSet = true;
		}
		if(Time.timeSinceLevelLoad >= splashDuration) {
			manager.LoadLevelByName("01a Start");
		}
	}
}
