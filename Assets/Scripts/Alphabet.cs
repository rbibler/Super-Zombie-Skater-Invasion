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
		lettersMap.Add ('A', 0);
		lettersMap.Add ('a', 1);
		lettersMap.Add ('B', 2);
		lettersMap.Add ('b', 3);
		lettersMap.Add ('C', 4);
		lettersMap.Add ('c', 5);
		lettersMap.Add ('D', 6);
		lettersMap.Add ('d', 7);
		lettersMap.Add ('E', 8);
		lettersMap.Add ('e', 9);
		lettersMap.Add ('F', 10);
		lettersMap.Add ('f', 11);
		lettersMap.Add ('G', 12);
		lettersMap.Add ('g', 13);
		lettersMap.Add ('H', 14);
		lettersMap.Add ('h', 15);
		lettersMap.Add ('I', 16);
		lettersMap.Add ('i', 17);
		lettersMap.Add ('J', 18);
		lettersMap.Add ('j', 19);
		lettersMap.Add ('K', 20);
		lettersMap.Add ('k', 21);
		lettersMap.Add ('L', 22);
		lettersMap.Add ('l', 23);
		lettersMap.Add ('M', 24);
		lettersMap.Add ('m', 25);
		lettersMap.Add ('N', 26);
		lettersMap.Add ('n', 27);
		lettersMap.Add ('O', 28);
		lettersMap.Add ('o', 29);
		lettersMap.Add ('P', 30);
		lettersMap.Add ('p', 31);
		lettersMap.Add ('Q', 32);
		lettersMap.Add ('q', 33);
		lettersMap.Add ('R', 34);
		lettersMap.Add ('r', 35);
		lettersMap.Add ('S', 36);
		lettersMap.Add ('s', 37);
		lettersMap.Add ('T', 38);
		lettersMap.Add ('t', 39);
		lettersMap.Add ('U', 40);
		lettersMap.Add ('u', 41);
		lettersMap.Add ('V', 42);
		lettersMap.Add ('v', 43);
		lettersMap.Add ('W', 44);
		lettersMap.Add ('w', 45);
		lettersMap.Add ('X', 46);
		lettersMap.Add ('x', 47);
		lettersMap.Add ('Y', 48);
		lettersMap.Add ('y', 49);
		lettersMap.Add ('Z', 50);
		lettersMap.Add ('z', 51);
		lettersMap.Add ('1', 52);
		lettersMap.Add ('2', 53);
		lettersMap.Add ('3', 54);
		lettersMap.Add ('4', 55);
		lettersMap.Add ('5', 56);
		lettersMap.Add ('6', 57);
		lettersMap.Add ('7', 58);
		lettersMap.Add ('8', 59);
		lettersMap.Add ('9', 60);
		lettersMap.Add ('0', 61);
		lettersMap.Add ('\'', 62);
		lettersMap.Add ('.', 63);
		lettersMap.Add ('-', 64);
		lettersMap.Add ('|', 65);
		lettersMap.Add (',', 66);
		lettersMap.Add ('!', 67);
		lettersMap.Add (':', 68);
		lettersMap.Add ('ñ', 69);
		lettersMap.Add (' ', 70);


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
