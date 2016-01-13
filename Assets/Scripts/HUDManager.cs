using UnityEngine;
using System.Collections;

public class HUDManager : MonoBehaviour {

	public HudDigit[] scoreDigits;
	public HudDigit[] coinDigits;
	public HudBar[] skaterHealth;
	public HudBar[] bossHealth;
	public HudBar[] slamMeter;

	public Sprite[] hudDigits;
	public Sprite[] hudMeterBars;


	public void UpdateHealth(float health) {
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
	
	
	public void updateScore(int score) {
		int hundredThousands = score / 100000;
		int tenThousands = (score % 100000) / 10000;
		int thousands = (score % 10000) / 1000;
		int hundreds = (score % 1000) / 100;
		int tens = (score % 100) / 10;
		int ones = score % 10;
		
		scoreDigits[0].GetComponent<SpriteRenderer>().sprite = hudDigits[hundredThousands];
		scoreDigits[1].GetComponent<SpriteRenderer>().sprite = hudDigits[tenThousands];
		scoreDigits[2].GetComponent<SpriteRenderer>().sprite = hudDigits[thousands];
		scoreDigits[3].GetComponent<SpriteRenderer>().sprite = hudDigits[hundreds];
		scoreDigits[4].GetComponent<SpriteRenderer>().sprite = hudDigits[tens];
		scoreDigits[5].GetComponent<SpriteRenderer>().sprite = hudDigits[ones];
	}
	
	public void updateCoins(int coinTally) {
		int hundredThousands = coinTally / 100000;
		int tenThousands = (coinTally % 100000) / 10000;
		int thousands = (coinTally % 10000) / 1000;
		int hundreds = (coinTally % 1000) / 100;
		int tens = (coinTally % 100) / 10;
		int ones = coinTally % 10;
		
		coinDigits[0].GetComponent<SpriteRenderer>().sprite = hudDigits[hundredThousands];
		coinDigits[1].GetComponent<SpriteRenderer>().sprite = hudDigits[tenThousands];
		coinDigits[2].GetComponent<SpriteRenderer>().sprite = hudDigits[thousands];
		coinDigits[3].GetComponent<SpriteRenderer>().sprite = hudDigits[hundreds];
		coinDigits[4].GetComponent<SpriteRenderer>().sprite = hudDigits[tens];
		coinDigits[5].GetComponent<SpriteRenderer>().sprite = hudDigits[ones];
	}
}
