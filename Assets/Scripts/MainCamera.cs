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
		float centerY = skater.renderer.bounds.center.y;
		if (centerY >= halfHeight) {
			pos.y = centerY;
			this.transform.position = pos;
		}
	}
}
