using UnityEngine;
using System.Collections;

public class ContinueScreenScript : MonoBehaviour {

	public LevelManager manager;
	public AudioClip clip;
	public Vector2 continueCursorPos;
	public Vector2 passwordCursorPos;
	public GameObject cursor;
	private bool password;

	void Update() {
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (!password) {
				cursor.transform.position = passwordCursorPos;
				password = true;
			} else {
				cursor.transform.position = continueCursorPos;
				password = false;
			}
		} else if (Input.GetKeyDown (KeyCode.Return)) {
			if(!password) {
				AudioSource.PlayClipAtPoint(clip, Vector3.left);
				Invoke("Continue", 2.0f);
			}
		}
	}
	
	void OnMouseDown() {
		AudioSource.PlayClipAtPoint(clip, continueCursorPos);
		Invoke("Continue", 2.0f);
	}
	
	void Continue() {
		manager.ReloadPreviousLevel();
	}


}
