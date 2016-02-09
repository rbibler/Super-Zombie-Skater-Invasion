using UnityEngine;
using System.Collections;

public class StartScreenButton : MonoBehaviour {

	public StartScreen startScreen;
	
	void OnMouseDown() {
		if(gameObject.name.Equals ("Password")) {
			startScreen.LoadPassword();
		} else {
			startScreen.FlashScreen ();
		}
	}
}
