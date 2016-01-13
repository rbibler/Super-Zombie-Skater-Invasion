using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof (MeshRenderer))]
public class BackgroundLayer : MonoBehaviour {

	public float xSpeed;
	public GameValues gameValues;
	
	private Material image;

	private float x;
	// Use this for initialization
	void Start () {
		image = GetComponent<MeshRenderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
		x += (xSpeed * Time.deltaTime * gameValues.speed);
		image.mainTextureOffset = Vector2.right * x;
	}
}
