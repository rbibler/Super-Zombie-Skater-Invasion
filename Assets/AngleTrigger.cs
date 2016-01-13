using UnityEngine;
using System.Collections;

public class AngleTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter2D(Collision2D col) {
		Skater skater = col.collider.gameObject.GetComponent<Skater>();
		if(!skater) {
			return;
		}
		skater.SetYPos(col.contacts[0].point.y);
	}
}
