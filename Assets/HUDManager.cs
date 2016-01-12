using UnityEngine;
using System.Collections;

public class HUDManager : MonoBehaviour {

public Sprite[] scoreDigits;
public Sprite[] hiDigits;
public Sprite[] coinDigits;
public Sprite[] skaterHealth;
public Sprite[] slamMeter;

public Sprite[] hudDigits;
public Sprite[] hudMeterBars;

	public void updateScore(int score) {
		int hundredThousands = score / 100000;
		int tenThousands = score / 10000;
		int thousands = score / 1000;
		int hundreds = score / 100;
		int tens = score / 10;
		int ones = score;
	}
}
