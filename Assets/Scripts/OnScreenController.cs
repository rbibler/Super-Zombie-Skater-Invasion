using UnityEngine;
using System.Collections;

public class OnScreenController : MonoBehaviour {

	public ControllerButton[] buttons;
	
	public GameObject dPad;
	public GameObject selectStart;
	public GameObject AB;

	private float dPadToBottom;
	private float selectStartToBottom;
	private float ABToBottom;

	private Vector3 dPadPos;
	private Vector3 selectStartPos;
	private Vector3 ABPos;

	void Start() {
		SetupView ();
		DetermineBottom ();
	}

	// Update is called once per frame
	void Update () {
		StickToBottom ();	
	}

	public int GetButtonState() {
		int count = 0;
		int retInt = 0;
		foreach (ControllerButton button in buttons) {
			if(button.GetButtonDown()) {
				retInt |= 1 << count;
			}
			count++;
		}
		return retInt;
	}

	void SetupView() {
		var dist = (dPad.transform.position - Camera.main.transform.position).z;
		Vector3 leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist));
		Vector3 rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist));
		dPadPos = dPad.transform.position;
		dPadPos.x = leftBorder.x + 4.5f;
		dPad.transform.position = dPadPos;
		ABPos = AB.transform.position;
		ABPos.x = rightBorder.x - 9.5f;
		AB.transform.position = ABPos;
		selectStartPos = selectStart.transform.position;

	}

	void DetermineBottom() {
		var dist = (dPad.transform.position - Camera.main.transform.position).z;
		Vector3 bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist));
		dPadToBottom = dPad.transform.position.y - bottomBorder.y;
		selectStartToBottom = selectStart.transform.position.y - bottomBorder.y;
		ABToBottom = AB.transform.position.y - bottomBorder.y;
	}

	void StickToBottom() {
		var dist = (dPad.transform.position - Camera.main.transform.position).z;
		Vector3 bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist));
		dPadPos.y = bottomBorder.y + dPadToBottom;
		dPad.transform.position = dPadPos;
		ABPos.y = bottomBorder.y + ABToBottom;
		AB.transform.position = ABPos;
		selectStartPos.y = bottomBorder.y + selectStartToBottom;
		selectStart.transform.position = selectStartPos;
	}
}
