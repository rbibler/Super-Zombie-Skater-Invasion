using UnityEngine;
using System.Collections;

public class GameValues : MonoBehaviour {

	public float speed;
	private float oldSpeed;

	public void SetSpeed(float speed) {
		oldSpeed = this.speed;
		this.speed = speed;
	}

	public void ResetSpeed() {
		speed = oldSpeed;
	}

}
