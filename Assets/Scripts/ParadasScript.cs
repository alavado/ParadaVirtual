using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ParadasScript : MonoBehaviour {

	bool zooming = false;
	const float zoomTime = 0.6f;	// Zoom time in seconds.
	float zoomTimer = 0f;			// Timer for zooming.

	public Image blackOverlay;		// Overlay for zooming.
	Color blackNoAlpha;				// Initial color for overlay lerp.

	public static int stopNumber = 1;

	void Start() {
		blackNoAlpha = new Color(0f, 0f, 0f, 0f);
	}

	void Update () {

		// Zoom to bus stop.
		if(zooming) {
			zoomTimer += Time.deltaTime;
			blackOverlay.color = Color.Lerp(blackNoAlpha, Color.black, zoomTimer / zoomTime);
			if(zoomTimer >= zoomTime) {
				SceneManager.LoadScene("Paradero");
			}
		}
	}

	// Called from the bus stop button.
	public void GoToStop(int stop) {
		stopNumber = stop;
		zooming = true;
	}
}
