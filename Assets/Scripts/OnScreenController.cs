using UnityEngine;
using System.Collections;

public class OnScreenController : MonoBehaviour {

	public ControllerButton[] buttons;

	// Update is called once per frame
	void Update () {
		
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
}
