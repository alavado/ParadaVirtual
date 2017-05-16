using UnityEngine;
using System.Collections;

public class FlyerScript : MonoBehaviour {

	public TextMesh titleText;
	public TextMesh contentText;

	public Texture[] backgrounds;
	public GameObject resizeObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetTitle(string title) {
		titleText.text = title;
	}

	public void SetContent(string content) {
		contentText.text = content;
	}//.Replace("\\n","\n");

	public void SetBackground(int index) {
		GetComponent<Renderer>().material.mainTexture = backgrounds[index];
	}

	public void ForbidResize() {
		resizeObject.SetActive(false);
	}
}
