using UnityEngine;
using System.Collections;

public class StartScreenButton : MonoBehaviour {


	public StartScreen startScreen;
	
	void OnMouseDown() {
		startScreen.FlashScreen ();
	}
}
