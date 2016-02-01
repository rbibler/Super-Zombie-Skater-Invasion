using UnityEngine;
using System.Collections;

public class PasswordManager : MonoBehaviour {

	public LevelManager manager;


	public bool CheckPassword(string password) {
		bool success = false;
		switch (password) {
		case "Start":
			TransitionToNextScene("01b Start Screen");
			success = true;
			break;

		case "UUDDLRLRABS":
			Skater.lives = 30;
			TransitionToNextScene("02a Level Splash 00");
			success = true;
			break;

		default:

			break;
		}
		return success;
	}

	void TransitionToNextScene(string scene) {
		manager.LoadLevelByNameWithDelay (scene, 2.0f);
	}
	
}
