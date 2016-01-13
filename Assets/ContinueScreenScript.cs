using UnityEngine;
using System.Collections;

public class ContinueScreenScript : MonoBehaviour {

	public LevelManager manager;
	public AudioClip clip;
	
	void OnMouseDown() {
		AudioSource.PlayClipAtPoint(clip, Vector3.left);
		Invoke("Continue", 2.0f);
	}
	
	void Continue() {
		manager.ReloadPreviousLevel();
	}
}
