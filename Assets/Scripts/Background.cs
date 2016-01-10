using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

	private bool left;
	private bool right;
	
	public BGLayer skyLayer;
	public BGLayer farLayer;
	public BGLayer midLayer;
	public BGLayer closeLayer;
	public BGLayer cloudLayer;
	
	// Use this for initialization
	void Start () {
	}
	
	public void SetInput(bool left, bool right) {
	skyLayer.SetInput(left, right);
	farLayer.SetInput(left, right);
	midLayer.SetInput(left, right);
	closeLayer.SetInput(left, right);
	cloudLayer.SetInput(left, right);
	}
}
