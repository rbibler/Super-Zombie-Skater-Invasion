using UnityEngine;
using System.Collections;

public class StartScreenButton : MonoBehaviour {

	public string levelToLoad;
	public LevelManager manager;
	
	void OnMouseDown() {
		manager.LoadLevelByName(levelToLoad);
	}
}
