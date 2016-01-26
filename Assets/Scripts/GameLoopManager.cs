using UnityEngine;
using System.Collections;

public class GameLoopManager : MonoBehaviour {
	
	public GameValues gameValues;
	public AnimatedFader fader;
	public LevelManager manager;
	public const int DEATH_RELOAD = 1;
	public const int DEATH_CONTINUE = 2;
	
	private int deathState;
	
	public void Die(int deathState) {
		gameValues.speed = 0;
		this.deathState = deathState;
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
