using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public static string previousLevel;
	public AnimatedFader fader;
	
	private string levelToLoadNext;
	private float fadeDelay;
	
	public void LoadNextLevel() {
		previousLevel = Application.loadedLevelName;
		Application.LoadLevel(levelToLoadNext);
		
	}
	
	public void LoadLevelByName(string name) {
		previousLevel = Application.loadedLevelName;
		Application.LoadLevel(name);
	}
	
	public void Quit() {
		print ("Quit");
	}
	
	public void SetNextLevel(string levelToLoadNext) {
		//print ("Setting Level to: " + levelToLoadNext);
		this.levelToLoadNext = levelToLoadNext;
	}
	
	public void SetFadeDelay(float fadeDelay) {
		//print ("Setting delay to: " + fadeDelay);
		this.fadeDelay = fadeDelay;
	}
	
	public void LoadWithFadeAndDelay(int faderType) {
		//print ("Loading level with fade type: " + faderType);
		fader.StartFadeAnimation(faderType);
		Invoke ("LoadNextLevel", fadeDelay);
	}
}
