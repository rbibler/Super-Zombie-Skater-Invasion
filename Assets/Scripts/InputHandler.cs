using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Serial))]
public class InputHandler : MonoBehaviour {
	
	private const int NES_A = 1;
	private const int NES_B = 2;
	private const int NES_SELECT = 4;
	private const int NES_START = 8;
	private const int NES_UP = 16;
	private const int NES_DOWN = 32;
	private const int NES_LEFT = 64;
	private const int NES_RIGHT = 128;


	bool a;
	bool b;
	bool select;
	bool start;
	bool up;
	bool down;
	bool left;
	bool right;
	bool aWasDown;

	private Serial serial;
	private OnScreenController controller;
	private ArrayList inputListeners = new ArrayList();
	private string inputLine;
	private bool touchScreen;

	// Use this for initialization
	void Start () {
		GameObject.DontDestroyOnLoad (gameObject);
		serial = GetComponent<Serial> ();
		serial.NotifyLines = true;
		FindController ();
		touchScreen = Input.touchSupported;
	}
	
	// Update is called once per frame
	void Update () {
		if (serial.SerialExistsAndOpen) {
			CheckSerialInput ();
		}
		if (controller != null && touchScreen) {
			int input = controller.GetButtonState ();
			HandleControllerInput (input);

		} else {
			a = Input.GetKeyDown (KeyCode.DownArrow);
			b = Input.GetKey (KeyCode.Space) || Input.GetKeyDown (KeyCode.Space);
			start = Input.GetKeyDown (KeyCode.Return) || (Input.touchCount > 0 && Input.touches [0].tapCount > 1);
			left = Input.GetKey (KeyCode.LeftArrow);
			right = Input.GetKey (KeyCode.RightArrow);
		}
		NotifyListeners ();
	}

	void CheckSerialInput() {
		if (inputLine == "") {
			return;
		}
		int input = -1;
		if (System.Int32.TryParse (inputLine, out input)) {
			HandleControllerInput(input);
		}
		inputLine = "";
	}

	void HandleControllerInput(int input) {
		bool aNew = (input & NES_A) != 0;
		bool bNew = (input & NES_B) != 0;
		select = (input & NES_SELECT) != 0;
		start = (input & NES_START) != 0;
		up = (input & NES_UP) != 0;
		down = (input & NES_DOWN) != 0;
		left = (input & NES_LEFT) != 0;
		right = (input & NES_RIGHT) != 0;
		b = bNew;
		a = !aWasDown && aNew;
		aWasDown = aNew;
	}
	
	void OnSerialLine(string line) {
		print (line);
		inputLine = line;
		return;
	}

	void OnLevelWasLoaded() {
		FindController ();

	}

	void FindController() {
		GameObject cont = GameObject.Find ("Controller");
		if (cont) {
			controller = cont.GetComponent<OnScreenController>();
		}
	}

	void NotifyListeners() {
		bool[] inputs = new bool[] {a, b, select, start, up, down, left, right};
		IInputListener listener;
		for (int i = 0; i < inputListeners.Count; i++) {
			listener = (IInputListener) inputListeners[i];
			listener.OnInput(inputs);
		}
	}

	public void RegisterListener(IInputListener listener) {
		inputListeners.Clear ();
		inputListeners.Add (listener);
	}

	public void UnregisterListener(IInputListener listener) {
		inputListeners.Remove (listener);
	}

}
