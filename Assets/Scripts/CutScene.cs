using UnityEngine;
using System.Collections;

public class CutScene : MonoBehaviour, IListener {

	public Alphabet alphabet;
	public Letter letter;
	public CutSceneCard[] cards;
	public AnimatedFader fader;

	private CutSceneCard currentCard;
	private int currentCardIndex;
	private bool waitingOnFader;

	void Start() {
		currentCard = cards [0];
		fader.RegisterListener (this);
	}

	void Update() {
		if (waitingOnFader) {
			return;
		}
		if (currentCard.DrawLine (alphabet, letter)) {
			fader.StartFadeAnimation(AnimatedFader.FADE_OUT);
			waitingOnFader = true;
		}
	}

	void IListener.OnNotification(string message) {
		if(message.Contains ("Fader Finished")) {
			message = message.Substring (message.IndexOf(":") + 1);
			print (message);
			int faderState = 0;
			int.TryParse(message, out faderState);
			if(faderState == 5) {
				waitingOnFader = false;
				fader.gameObject.SetActive(false);
				GoToNextCard();
			}
		}
	}

	void GoToNextCard() {
		Destroy (currentCard.gameObject);
		currentCardIndex++;
		currentCard = cards [currentCardIndex];
	}

	

}
