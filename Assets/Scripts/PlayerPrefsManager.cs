using UnityEngine;
using System.Collections;

public class PlayerPrefsManager : MonoBehaviour {

	const string MASTER_VOLUME_KEY = "master_volume";
	const string DIFFICULTY_KEY = "difficulty";
	const string LEVEL_KEY = "level_unlocked_";

	public static void SetMasterVolume(float volume) {
		if (volume > 0f && volume < 1f) {
			PlayerPrefs.SetFloat (MASTER_VOLUME_KEY, volume);
		} else {
			Debug.LogError ("Master volume out of range");
		}
	}

	public static float GetMasterVolume() {
		return PlayerPrefs.GetFloat (MASTER_VOLUME_KEY);
	}

	public static void UnlockLevel(int level) {
		if (level >= 0 && level <= Application.levelCount - 1) {
			PlayerPrefs.SetInt (LEVEL_KEY + level.ToString(), 1);
		} else {
			Debug.LogError ("Trying to unlock a nonexistent level");
		}
	}

	public static bool IsLevelUnlocked(int level) {
		bool ret = false;
		int levelLock;
		if (level <= Application.levelCount - 1) {
			levelLock = PlayerPrefs.GetInt (LEVEL_KEY + level.ToString());
			if(levelLock == 1) {
				ret = true;
			} 
		} else {
			Debug.LogError ("Trying to check lock status on a nonexistent level");
		}
		return ret;
	}

	public static void SetDifficulty(float difficulty) {
		if (difficulty >= 0f && difficulty <= 1.0f) {
			PlayerPrefs.SetFloat (DIFFICULTY_KEY, difficulty);
		} else {
			Debug.LogError("Trying to Set Too Large Difficulty");
		}
	}

	public static float GetDifficulty() {
		return PlayerPrefs.GetFloat (DIFFICULTY_KEY);
	}
}
