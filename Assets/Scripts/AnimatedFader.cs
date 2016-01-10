using UnityEngine;
using System.Collections;

public class AnimatedFader : MonoBehaviour {

	public const int SLIDE_DOWN = 0;
	public const int FADE_IN = 1;
	public const int SLIDE_IN_RIGHT = 2;
	public const int SLIDE_DIAGONAL_RIGHT = 3;
	public const int SLIDE_UP = 4;
	public const int FADE_OUT = 5;
	public const int SLIDE_IN_LEFT = 6;
	public const int SLIDE_DIAGONAL_LEFT = 7;
	public const int SLIDE_OUT_LEFT = 8;
	
	public int startState;
	
	private Animator animator;

	// Use this for initialization
	void Awake () {
		animator = GetComponent<Animator>();
		animator.SetInteger("state", startState);
		
	}
	
	public void StartFadeAnimation (int state) {
		//print ("Setting state to: " + state);
		gameObject.SetActive (true);
		animator.SetInteger("state", state);
	}
	
	public void Deactivate() {
		gameObject.SetActive(false);
	}
	
	public void NotifyStart() {
		//print ("Starting Fade OUt anim");
	}
}
