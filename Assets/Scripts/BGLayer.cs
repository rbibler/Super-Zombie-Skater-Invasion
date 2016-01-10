using UnityEngine;
using System.Collections;

public class BGLayer : MonoBehaviour {

	private ArrayList imageTransforms;
	private ArrayList imagePositions;
	
	private bool left;
	private bool right;
	
	public float updateSpeed;
	public float width = 26;
			
	// Use this for initialization
	void Start () {
		imageTransforms = new ArrayList();
		imagePositions = new ArrayList();
		Component[] transforms = GetComponentsInChildren<Transform>();
		foreach (Transform transform in transforms) {
			if(transform != this.transform) {
				imageTransforms.Add (transform);
				imagePositions.Add (new Vector3(transform.position.x, transform.position.y, transform.position.z));
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		HandleInput();
		CheckForWrap();
		UpdateImageTransforms();
	}
	
	public void HandleInput() {
		float deltaMovement = 0;
		if(left) {
			deltaMovement = updateSpeed;
		} else if(right) {
			deltaMovement = -updateSpeed;
		}
		UpdateImagePositions(deltaMovement);
	}
	
	private void UpdateImagePositions(float deltaMovement) {
		Vector3 pos;
		for(int i = 0; i < imagePositions.Count; i++) {
			pos = (Vector3) imagePositions[i];
			pos.x += deltaMovement;
			imagePositions[i] = pos;
		}
	}
	
	private void UpdateImageTransforms() {
		for(int i = 0; i < imageTransforms.Count; i++) {
			((Transform)imageTransforms[i]).position = (Vector3) imagePositions[i];
		}
	}
	
	public void SetInput(bool left, bool right) {
		this.left = left;
		this.right = right;
	}
	
	void CheckForWrap() {
		Vector3 pos;
		for(int i = 0; i < imagePositions.Count; i++) {
			pos = (Vector3) imagePositions[i];
			if(pos.x + width <= 0) {
				pos.x = width;
				imagePositions[i] = pos;
			}
		}
	}
}
