using UnityEngine;
using System.Collections;

public class Alphabet : MonoBehaviour {

	public Sprite[] letters;

	private Hashtable lettersMap;


	
	// Update is called once per frame
	void Update () {
	
	}

	void SetupLetters() {
		lettersMap = new Hashtable();
		lettersMap.Add (' ', 0);
		lettersMap.Add ('0', 1);
		lettersMap.Add ('1', 2);
		lettersMap.Add ('2', 3);
		lettersMap.Add ('3', 4);
		lettersMap.Add ('4', 5);
		lettersMap.Add ('5', 6);
		lettersMap.Add ('6', 7);
		lettersMap.Add ('7', 8);
		lettersMap.Add ('8', 9);
		lettersMap.Add ('9', 10);
		lettersMap.Add ('A', 11);
		lettersMap.Add ('B', 12);
		lettersMap.Add ('C', 13);
		lettersMap.Add ('D', 14);
		lettersMap.Add ('E', 15);
		lettersMap.Add ('F', 16);
		lettersMap.Add ('G', 17);
		lettersMap.Add ('H', 18);
		lettersMap.Add ('I', 19);
		lettersMap.Add ('J', 20);
		lettersMap.Add ('K', 21);
		lettersMap.Add ('L', 22);
		lettersMap.Add ('M', 23);
		lettersMap.Add ('N', 24);
		lettersMap.Add ('O', 25);
		lettersMap.Add ('P', 26);
		lettersMap.Add ('Q', 27);
		lettersMap.Add ('R', 28);
		lettersMap.Add ('S', 29);
		lettersMap.Add ('T', 30);
		lettersMap.Add ('U', 31);
		lettersMap.Add ('V', 32);
		lettersMap.Add ('W', 33);
		lettersMap.Add ('X', 34);
		lettersMap.Add ('Y', 35);
		lettersMap.Add ('Z', 36);
		lettersMap.Add ('.', 37);
		lettersMap.Add ('-', 38);
		lettersMap.Add ('*', 39);
		lettersMap.Add ('?', 40);
		lettersMap.Add ('!', 41);
		lettersMap.Add ('[', 42);
		lettersMap.Add (']', 43);
		lettersMap.Add ('\'', 44);
		lettersMap.Add (':', 45);
		lettersMap.Add (',', 46);
	}

	public Sprite GetLetter(char letter) {
		if (lettersMap == null) {
			SetupLetters();
		}
		int spriteNum = (int) lettersMap [letter];
		if(spriteNum >= 0) {
			return letters[spriteNum];
		}
		return letters [0];
	}
}
