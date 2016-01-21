using UnityEngine;
using System.Collections;

public class GameLoopManager : MonoBehaviour {

	public AudioClip deathClip;
	public GameValues gameValues;
	public AnimatedFader fader;
	public LevelManager manager;
	public Song levelAudioSource;
	public float deathPause;
	public const int DEATH_RELOAD = 1;
	public const int DEATH_CONTINUE = 2;
	
	private int deathState;
	private float timeOfDeath;
	
	
	// Update is called once per frame
	void Update () {
		
		if(deathState == 0) {
			return;
		}
		float timeSinceDeath = Time.timeSinceLevelLoad - timeOfDeath;
		if(timeSinceDeath >= deathPause) {
			switch(deathState) {
			case DEATH_RELOAD:
				manager.ReloadCurrentLevel();
				break;
			case DEATH_CONTINUE:
				manager.LoadLevelByName("03a Continue");
				break;
			}
			deathState = 0;
		} 
	}
	
	public void Die(int deathState) {
		timeOfDeath = Time.timeSinceLevelLoad;
		gameValues.speed = 0;
		this.deathState = deathState;
		levelAudioSource.Stop();
		AudioSource.PlayClipAtPoint(deathClip, Vector3.left);
	}
}
