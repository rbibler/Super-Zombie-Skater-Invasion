using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof (RawImage))]
public class BackgroundLayer : MonoBehaviour {

	public float xSpeed;
	
	private RawImage image;
	private Rect rect;
	// Use this for initialization
	void Start () {
		image = GetComponent<RawImage>();
		rect = image.uvRect;
	}
	
	// Update is called once per frame
	void Update () {
		rect.x += (xSpeed * Time.deltaTime);
		image.uvRect = rect;
	}
}
