using UnityEngine;
using System.Collections;

public class ShowStarsScript : MonoBehaviour {

	public GameObject starsUI;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			// Resize button first.
			if(Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("StarsPanel"))) {
				starsUI.SetActive(true);
				StarScript.isActive = true;
			}
		}
	}
}
