using UnityEngine;
using UnityEngine.UI;
using System.Collections;
[RequireComponent (typeof (MeshRenderer))]
public class BackgroundLayer : MonoBehaviour {

	public float xSpeed;
	public float ySpeed;
	public GameValues gameValues;
	public Material[] materials;
	public float materialSwapAnimTime;
	
	private Material currentMaterial;
	private int currentMaterialIndex;
	private MeshRenderer renderer;

	private float x;
	private float y;
	private float lastSwapTime;

	void Start() {
		renderer = GetComponent<MeshRenderer> ();
		SwapFrame ();

	}


	
	// Update is called once per frame
	void Update () {
		if (Time.timeSinceLevelLoad - lastSwapTime > materialSwapAnimTime) {
			SwapFrame();
		}
		x += (xSpeed * Time.deltaTime * gameValues.speed);
		y += (ySpeed * Time.deltaTime);
		currentMaterial.mainTextureOffset = new Vector2(x,y);
	}

	void SwapFrame() {
		currentMaterial = materials [currentMaterialIndex++];
		if (currentMaterialIndex >= materials.Length) {
			currentMaterialIndex = 0;
		}
		renderer.material = currentMaterial;
		lastSwapTime = Time.timeSinceLevelLoad;
	}
}
