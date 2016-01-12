using UnityEngine;
using System.Collections;

public class HUDManager : MonoBehaviour {

public HudDigit[] scoreDigits;
public HudDigit[] hiDigits;
public HudDigit[] coinDigits;
private Sprite[] skaterHealth;
private Sprite[] slamMeter;

public Sprite[] hudDigits;
public Sprite[] hudMeterBars;

private int score = 0;
private int hiScore = 0;
private int coins = 0;

	
	
	void Update() {
		updateScore (score++);
		updateHiScore (hiScore += 13);
		updateCoins(coins++);
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
	
	public void updateHiScore(int score) {
		int hundredThousands = score / 100000;
		int tenThousands = (score % 100000) / 10000;
		int thousands = (score % 10000) / 1000;
		int hundreds = (score % 1000) / 100;
		int tens = (score % 100) / 10;
		int ones = score % 10;
		
		hiDigits[0].GetComponent<SpriteRenderer>().sprite = hudDigits[hundredThousands];
		hiDigits[1].GetComponent<SpriteRenderer>().sprite = hudDigits[tenThousands];
		hiDigits[2].GetComponent<SpriteRenderer>().sprite = hudDigits[thousands];
		hiDigits[3].GetComponent<SpriteRenderer>().sprite = hudDigits[hundreds];
		hiDigits[4].GetComponent<SpriteRenderer>().sprite = hudDigits[tens];
		hiDigits[5].GetComponent<SpriteRenderer>().sprite = hudDigits[ones];
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
