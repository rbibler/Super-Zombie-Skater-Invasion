using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animator))]

public class StartScreen : MonoBehaviour {

	public AudioClip cymbolCrash;
	public LevelManager manager;
	public AudioSource audioSource;
	public AnimatedFader fader;

	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	public void FlashScreen() {
		audioSource.Stop ();
		AudioSource.PlayClipAtPoint (cymbolCrash, Vector3.left);
		animator.SetTrigger ("Flash");
	}

	public void StartFader() {
		fader.StartFadeAnimation (AnimatedFader.FADE_OUT);
	}

	public void LoadFirstLevel() {
		manager.LoadLevelByName ("02a Level Splash 00");
	}
}
