using UnityEngine;
using System.Collections;

public class PlaySpace : MonoBehaviour {

	Vector3 pos;
	private bool left;
	private bool right;

	void Start () {
		pos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		HandleInput ();
		transform.position = pos;
	}

	public void SetInput(bool left, bool right) {
		this.left = left;
		this.right = right;
	}

	void HandleInput() {
		if (left) {
			pos.x += .1f;
		} else if (right) {
			pos.x -= .1f;
		}
	}
}
