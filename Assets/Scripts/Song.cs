using UnityEngine;
using System.Collections;

public class Song : MonoBehaviour {

	private AudioSource sourceOne;
	private AudioSource sourceTwo;
	public AudioClip songClip;
	public int introEndInSamples;
	public int loopInSamples;


	private double introEndTime;
	private double nextLoopSwap;
	private bool sourceOneOn;
	private double oneSampleInSeconds;
	private double loopTime;
	// Use this for initialization
	void Awake() {
		sourceOne = gameObject.AddComponent<AudioSource> ();
		sourceOne.clip = songClip;
		oneSampleInSeconds = 1.0 / songClip.frequency;
		loopTime = loopInSamples * oneSampleInSeconds;
		introEndTime = AudioSettings.dspTime + (introEndInSamples * oneSampleInSeconds);
		nextLoopSwap = introEndTime + loopTime;
		sourceOne.PlayScheduled(AudioSettings.dspTime);
		sourceOne.SetScheduledEndTime (introEndTime);
		Invoke ("SetupLoop", 2.0f);
	}

	void SetupLoop() {
		sourceTwo = gameObject.AddComponent<AudioSource> ();
		sourceTwo.playOnAwake = false;
		sourceTwo.clip = songClip;
		sourceTwo.timeSamples = introEndInSamples;
		sourceTwo.PlayScheduled (introEndTime);
		sourceTwo.SetScheduledEndTime (nextLoopSwap);
		InvokeRepeating ("SwapLoop", (float) ((introEndInSamples * oneSampleInSeconds) + 1.0f), (float) loopTime);
	}

	void SwapLoop() {

		if (sourceOneOn) {
			sourceTwo.timeSamples = introEndInSamples;
			sourceTwo.PlayScheduled (nextLoopSwap);
			nextLoopSwap += loopTime;
			sourceTwo.SetScheduledEndTime(nextLoopSwap);
		} else {
			sourceOne.timeSamples = introEndInSamples;
			sourceOne.PlayScheduled(nextLoopSwap);
			nextLoopSwap += loopTime;
			sourceOne.SetScheduledEndTime(nextLoopSwap);
		}
		sourceOneOn = !sourceOneOn;
	}

	public void Stop() {
		if (sourceOne) {
			sourceOne.Stop ();
		}
		if (sourceTwo) {
			sourceTwo.Stop ();
		}
	}

}
