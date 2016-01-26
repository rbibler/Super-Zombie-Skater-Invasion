using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

	public AudioClip collectionClip;
	

	void OnTriggerEnter2D(Collider2D col) {
		Skater skater = col.gameObject.GetComponent<Skater> ();
		Coin coin = GetComponent<Coin> ();
		if(coin || !skater) {
			return;
		}
		AudioSource.PlayClipAtPoint (collectionClip, transform.position);
	}
}
