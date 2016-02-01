using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animator))]

public class StartScreen : MonoBehaviour, IInputListener {

	public AudioClip cymbolCrash;
	public LevelManager manager;
	public AudioSource audioSource;
	public AnimatedFader fader;
	public Vector2[] cursorPositions;
	public GameObject cursor;

	private int currentCursor;

	private Animator animator;
	private InputHandler handler;
	private float lastSwitch;
	private float delay = .25f;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		SetupInputHandler ();
		currentCursor = 0;
		lastSwitch = Time.timeSinceLevelLoad;
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

	public void OnInput(bool[] inputs) {
		if (inputs [2]) {
			SwitchCursor();
		}
		if (inputs [3]) {
			ExecuteSelection();
		}
	}

	public void LoadPassword() {
		manager.LoadLevelByName("01c Password");
	}

	void SwitchCursor() {
		if (Time.timeSinceLevelLoad - lastSwitch < delay) {
			return;
		}
		lastSwitch = Time.timeSinceLevelLoad;
		if (currentCursor == 0) {
			currentCursor = 1;
		} else {
			currentCursor = 0;
		}
		cursor.transform.position = cursorPositions [currentCursor];
	}

	void ExecuteSelection() {
		if (currentCursor == 0) {
			FlashScreen ();
		} else {
			LoadPassword();
		}
	}
}
