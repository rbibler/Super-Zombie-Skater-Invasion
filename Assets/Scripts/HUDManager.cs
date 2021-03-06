﻿using UnityEngine;
using System.Collections;

public class HUDManager : MonoBehaviour {

	public HudDigit[] scoreDigits;
	public HudDigit[] coinDigits;
	public HudDigit[] playerLives;
	public HudBar[] skaterHealth;
	public HudBar[] bossHealth;
	public HudBar[] slamMeter;

	public Sprite[] hudDigits;
	public Sprite[] hudMeterBars;
	public HudDigit worldNumber;
	public HudDigit stageNumber;

	private Vector3 pos;

	void Start() {
		pos = transform.position;
		SetupLevelName ();
	}

	void Update() {
		pos.x = Camera.main.transform.position.x - 10.50429f;
		pos.y = Camera.main.transform.position.y - 10.50429f;
		transform.position = pos;
	}



	public void UpdateHealth(float health) {
		if(health <= 0) {
			health = 0;
		}
		
		int fullBars = (int) (14 * health);
		int whichBar;
		for (int i = 0; i < skaterHealth.Length; i++) {
			whichBar = 0;
			if(i < fullBars) {
				whichBar = 1;
			}
			skaterHealth[i].GetComponent<SpriteRenderer>().sprite = hudMeterBars[whichBar];
		}

	}

	public void UpdateSlam(float slam) {
		int fullBars = (int) (14 * slam);
		int whichBar;
		for (int i = 0; i < slamMeter.Length; i++) {
			whichBar = 0;
			if(i < fullBars) {
				whichBar = 1;
			}
			slamMeter[i].GetComponent<SpriteRenderer>().sprite = hudMeterBars[whichBar];
		}
		
	}

	public void UpdateBossHealth(float health) {
		int fullBars = (int) (14 * health);
		int whichBar;
		for (int i = 0; i < bossHealth.Length; i++) {
			whichBar = 0;
			if(i < fullBars) {
				whichBar = 1;
			}
			bossHealth[i].GetComponent<SpriteRenderer>().sprite = hudMeterBars[whichBar];
		}
		
	}
	
	public void UpdateScore(float score) {
		return;
		int hundredThousands = (int) (score / 100000);
		int tenThousands = (int) ((score % 100000) / 10000);
		int thousands = (int) ((score % 10000) / 1000);
		int hundreds = (int) ((score % 1000) / 100);
		int tens = (int) ((score % 100) / 10);
		int ones = (int) (score % 10);
		
		scoreDigits[0].GetComponent<SpriteRenderer>().sprite = hudDigits[hundredThousands];
		scoreDigits[1].GetComponent<SpriteRenderer>().sprite = hudDigits[tenThousands];
		scoreDigits[2].GetComponent<SpriteRenderer>().sprite = hudDigits[thousands];
		scoreDigits[3].GetComponent<SpriteRenderer>().sprite = hudDigits[hundreds];
		scoreDigits[4].GetComponent<SpriteRenderer>().sprite = hudDigits[tens];
		scoreDigits[5].GetComponent<SpriteRenderer>().sprite = hudDigits[ones];
	}
	
	public void UpdateCoins(int coinTally) {

		int tens = (coinTally % 100) / 10;
		int ones = coinTally % 10;
		coinDigits[0].GetComponent<SpriteRenderer>().sprite = hudDigits[tens];
		coinDigits[1].GetComponent<SpriteRenderer>().sprite = hudDigits[ones];
	}
	
	public void UpdateLives(int lives) {
		int tens = lives / 10;
		int ones = lives % 10;
		playerLives[0].GetComponent<SpriteRenderer>().sprite = hudDigits[tens];
		playerLives[1].GetComponent<SpriteRenderer>().sprite = hudDigits[ones];
	}

	void SetupLevelName() {
		string levelName = Application.loadedLevelName;
		string worldStage = levelName.Substring (levelName.Length - 4);
		print (worldStage);
		int world = 0;
		int stage = 0;
		System.Int32.TryParse (worldStage.Substring (0, 2), out world);
		System.Int32.TryParse (worldStage.Substring (2), out stage);
		worldNumber.GetComponent<SpriteRenderer> ().sprite = hudDigits [world];
		stageNumber.GetComponent<SpriteRenderer> ().sprite = hudDigits [stage];
	}

}
