using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

	public Skater skater;
	private Vector3 pos;
	private float height;
	private float halfHeight;

	// Use this for initialization
	void Start () {
		halfHeight = Camera.main.orthographicSize;
		height = halfHeight * 2;
		pos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (!skater) {
			return;
		}
		float centerY = skater.transform.position.y;
		if (centerY >= 10.50429f) {
			pos.y = centerY;

		} else if(centerY <= 10.50429f) {
			pos.y = 10.50429f;
		}
		this.transform.position = pos;
	}
}
