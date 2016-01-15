using UnityEngine;
using System.Collections;

public class Song : MonoBehaviour {

	private AudioSource introSource;
	private AudioSource loopSource;
	public AudioClip loop;
	public AudioClip intro;
	// Use this for initialization
	void Awake() {
		introSource = gameObject.AddComponent<AudioSource> ();
		loopSource = gameObject.AddComponent<AudioSource> ();
		introSource.clip = intro;
		introSource.Play ();
		loopSource.loop = true;
		loopSource.clip = loop;
		loopSource.PlayScheduled (AudioSettings.dspTime + intro.length - (2.0 / intro.frequency));
	}

}
