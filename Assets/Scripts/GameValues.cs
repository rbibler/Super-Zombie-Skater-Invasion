using UnityEngine;
using System.Collections;

public class GameValues : MonoBehaviour {

	public float speed;
	public float defaultSpeed;
	private float oldSpeed;
	
	void Start() {
		speed = defaultSpeed;
	}

	public void SetSpeed(float speed) {
		oldSpeed = this.speed;
		this.speed = speed;
	}

	public void ResetSpeed() {
		speed = oldSpeed;
	}

}
