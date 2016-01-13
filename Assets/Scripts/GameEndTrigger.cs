using UnityEngine;
using System.Collections;

public class GameEndTrigger : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col) {
		Skater skater = col.gameObject.GetComponent<Skater>();
		if(!skater) {
			return;
		}
		skater.Die ();
	}
}
