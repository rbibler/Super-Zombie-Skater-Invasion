using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	public AudioClip[] musicOrderChangeArray;
	
	private AudioSource audioSource;

	void Awake() {
		DontDestroyOnLoad(gameObject);
		audioSource = GetComponent<AudioSource>();
		//ChangeVolume (PlayerPrefsManager.GetMasterVolume ());

	}
	
	void OnLevelWasLoaded(int level) {
		LoadAudioClip (level);
	}

	public void ChangeVolume(float volume) {
		if (volume >= 0 && volume <= 1.0f) {
			audioSource.volume = volume;
		} else {
			Debug.LogError("Volume out of range");
		}
	}

	private void LoadAudioClip(int clipToLoad) {
		AudioClip thisLevelsMusic = musicOrderChangeArray[clipToLoad];

		if(thisLevelsMusic) {
			audioSource.clip = thisLevelsMusic;
			audioSource.loop = true;
			audioSource.Play ();
		}
	}
}
