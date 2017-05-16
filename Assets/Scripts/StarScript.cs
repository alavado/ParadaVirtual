using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StarScript : MonoBehaviour {

	public Image[] stars;
	public string[] words;
	public Sprite star, star_empty;
	public Text ratingText;
	public Button[] busesButtons;
	public GameObject starsUI;
	static public bool isActive = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Rate(int pos) {
		for(int i = 0; i <= pos; i++) {
			stars[i].sprite = star;
		}
		for(int i = pos + 1; i < stars.Length; i++) {
			stars[i].sprite = star_empty;
		}
		ratingText.text = words[pos];
	}

	public void ResetStars(int pos) {

		for(int i = 0; i < busesButtons.Length; i++) {
			busesButtons[i].GetComponent<Outline>().enabled = i == pos;
		}

		for(int i = 0; i < stars.Length; i++) {
			stars[i].sprite = star_empty;
		}
		ratingText.text = "Not rated yet";
	}

	public void CloseStars() {
		isActive = false;
		starsUI.SetActive(false);
	}
}
