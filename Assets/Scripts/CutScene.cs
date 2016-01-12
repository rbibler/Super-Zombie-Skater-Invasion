using UnityEngine;
using System.Collections;


public class CutScene : MonoBehaviour, IListener {

	public Alphabet alphabet;
	public Letter letter;
	public CutSceneCard[] cards;
	public AnimatedFader fader;
	public string levelToLoad;
	public LevelManager manager;

	private CutSceneCard currentCard;
	private int currentCardIndex;
	private bool waitingOnFader;
	private bool finishOnNextFade;

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
			int faderState = 0;
			int.TryParse(message, out faderState);
			if(faderState == 5) {
				if(finishOnNextFade) {
					manager.LoadLevelByName(levelToLoad);
					return;
				}
				waitingOnFader = false;
				fader.gameObject.SetActive(false);
				GoToNextCard();
			}
		}
	}

	void GoToNextCard() {
		Destroy (currentCard.gameObject);
		currentCardIndex++;
		if(currentCardIndex == cards.Length - 1) {
			finishOnNextFade = true;
		}
		currentCard = cards [currentCardIndex];
	}

	void OnMouseDown() {
		EndScene ();
	}

	void EndScene() {
		manager.LoadLevelByName(levelToLoad);
	}

	

}
