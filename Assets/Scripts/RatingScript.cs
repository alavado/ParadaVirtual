using UnityEngine;
using System.Collections;

public class RatingScript : MonoBehaviour {
	private bool ratingOn = false;
	public GameObject ratingPanel;

	void OnMouseDown() {
		if (ratingOn) {
			ratingOn = false;
			ratingPanel.SetActive(false);
		}
		else{
			ratingOn = true;
			ratingPanel.SetActive(true);
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
