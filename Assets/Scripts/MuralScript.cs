using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MuralScript : MonoBehaviour {

	private bool zoomed = false;

	public Transform target;
	public float smooth = 5.0f;
	private Vector3 velocity = new Vector3(30.0f, 30.0f, 30.0f);
	private bool showGUI = false, volver = false;

	private float zoom_x = 8.1f, zoom_y = 13.7f, zoom_z = 35.2f;
	private float def_x = 35.2f, def_y = 20.0f, def_z = 29.4f;

	private bool postitcreated = false;		// ?

	private TouchScreenKeyboard keyboard;

	public float z_post = -2.0f;			// Z coordinate for floating flyers.
	public Button mapButton;				// Button that takes you back to the map.

	public float gridWidth, gridHeight;		// Width and height of the billboard.
	public GameObject topLeft, topRight,	// Empty game objects that mark the limits of the billboard.
		bottomLeft, bottomRight;

	public Button addFlyerButton;			// Button to show add flyer interface.
	public GameObject addFlyerUI;			// Add flyer interface.
	public Image addFlyerUIBackground;
	public Sprite[] addFlyerBackgrounds;
	int addFlyerBackgroundIndex = 0;
	public Button pasteFlyerButton;

	float flyerOriginalWidth;
	float flyerOriginalHeight;

	public InputField titleText;
	public InputField contentText;

	Vector3 flyerOriginalScale;
	public GameObject pinsToPaste;
	public TextMesh pinstToPasteText;

	public Text moneyText;					// Where money is shown in the UI.
	int coins;
	float flyersZOffset = 0.01f;

	public GameObject dog;
	public GameObject shop;
	public GameObject youtube;

	public int pinCount = 100;
	void Start() {
		target = Camera.main.transform;
		gridWidth = topRight.transform.position.z - topLeft.transform.position.z;
		gridHeight = topRight.transform.position.y - bottomRight.transform.position.y;
		coins = int.Parse(moneyText.text);
		dog.SetActive(ParadasScript.stopNumber == 2);
		shop.SetActive(ParadasScript.stopNumber == 1);
	}

	void Update() {
		if(zoomed) {
			showGUI = true;
			Vector3 targetPosition = new Vector3(zoom_x, zoom_y, zoom_z);
			target.position = Vector3.Slerp(target.position, targetPosition, Time.deltaTime * smooth);
		}
		if(volver) {
			showGUI = false;
			Vector3 targetPosition = new Vector3(def_x, def_y, def_z);
			target.position = Vector3.Slerp(target.position, targetPosition, Time.deltaTime * smooth);
			mapButton.gameObject.SetActive(true);
			addFlyerButton.gameObject.SetActive(false);
			if (Mathf.Abs (target.position.x - def_x) < 0.5) {
				volver = false;
			}
		}

		// Initiate placing and resizing the flyer.
		if(Input.GetMouseButton(0) && !resizing) {
			if(placingFlyer) {
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;

				// Resize button first.
				if(Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("ResizeButtons"))) {
					resizing = true;
					placingFlyer = false;
				}
				else if(Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Mural"))) {
					Vector3 posit_pos = new Vector3(hit.point.x + flyersZOffset, hit.point.y, hit.point.z);
					currentFlyer.transform.position = posit_pos;
					Bounds boundsFlayer = currentFlyer.GetComponent<Renderer>().bounds;
					pinsToPaste.transform.position = currentFlyer.transform.position - new Vector3(-0.01f - flyersZOffset, -boundsFlayer.extents.y, -boundsFlayer.extents.z);
				}
			}
		}

		// Resize is active.
		if(resizing) {
			if(Input.GetMouseButton(0)) {
				ResizeFlyer();
			}
			else {
				resizing = false;
				placingFlyer = true;
			}
		}
	}

	void ResizeFlyer() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Mural"))) {
			Bounds boundsBeforeResize = currentFlyer.GetComponent<Renderer>().bounds;
			float newWidth = hit.point.z - boundsBeforeResize.center.z + boundsBeforeResize.extents.z;
			float newHeight = boundsBeforeResize.center.y - hit.point.y + boundsBeforeResize.extents.y;
			float scaleZ = newWidth / flyerOriginalWidth;
			float scaleX = newHeight / flyerOriginalHeight;
			currentFlyer.transform.localScale = new Vector3(flyerOriginalScale.x * scaleX, flyerOriginalScale.y, flyerOriginalScale.z * scaleZ);
			Bounds boundsAfterResize = currentFlyer.GetComponent<Renderer>().bounds;
			float deltaY = (boundsAfterResize.extents.y - boundsBeforeResize.extents.y);
			float deltaZ = (boundsAfterResize.extents.z - boundsBeforeResize.extents.z);
			currentFlyer.transform.position += new Vector3(0f, -deltaY, deltaZ);
			pinsToPaste.transform.position = currentFlyer.transform.position - new Vector3(-flyersZOffset, -boundsAfterResize.extents.y, -boundsAfterResize.extents.z);

			flyerPrice = (int)Mathf.Max(1, Mathf.Round(1 + 10 * ((currentFlyer.transform.localScale.x - flyerOriginalScale.x) + (currentFlyer.transform.localScale.z - flyerOriginalScale.z))));
			pinstToPasteText.text = flyerPrice.ToString();

			// Update money on the UI.
			moneyText.text = coins.ToString();
		}
	}

	void OnGUI() {
		if (showGUI) {
			if (GUI.Button (new Rect (10, 10, 100, 100), "Volver")) {
				showGUI = false;
				volver = true;
				zoomed = false;
			}
		}
		if(keyboard != null && keyboard.done) {
			GUI.Label (new Rect(50, 50, 100, 20),  keyboard.text);
		}
	}

	bool placingFlyer = false;
	bool resizing = false;
	GameObject currentFlyer = null;
	int flyerPrice = 0;

	void OnMouseDown() {
		if (!zoomed && !StarScript.isActive) {
			volver = false;
			zoomed = true;
			addFlyerButton.gameObject.SetActive(ParadasScript.stopNumber == 1);
		}
	}

	public void ShowFlyerInterface() {
		addFlyerUI.SetActive(true);
	}

	public void HideFlyerInterface() {
		addFlyerUI.SetActive(false);
	}

	public void AddNewFlyer() {
		HideFlyerInterface();
		// Create flyer on the board in some place
		Quaternion rot = Quaternion.Euler (0.0f, 0.0f, -90.0f);
		GameObject postit = Instantiate(GameObject.Find ("Post"));
		Vector3 postit_pos = new Vector3 (-8.24f, 19.76f, 30.68f);
		postit.transform.position = postit_pos;
		currentFlyer = postit;
		Bounds boundsFlayer = currentFlyer.GetComponent<Renderer>().bounds;
		flyerOriginalWidth = boundsFlayer.extents.z * 2f;
		flyerOriginalHeight = boundsFlayer.extents.y * 2f;
		print(currentFlyer.GetComponent<Renderer>().bounds.extents);
		flyerOriginalScale = currentFlyer.transform.localScale;

		// Fill flyer.
		postit.GetComponent<FlyerScript>().SetTitle(titleText.text);
		postit.GetComponent<FlyerScript>().SetContent(contentText.text);
		postit.GetComponent<FlyerScript>().SetBackground(addFlyerBackgroundIndex);

		pinsToPaste.transform.position = currentFlyer.transform.position - new Vector3(-0.01f - flyersZOffset, -boundsFlayer.extents.y, -boundsFlayer.extents.z);
		
		placingFlyer = true;
		addFlyerButton.gameObject.SetActive(false);
		pasteFlyerButton.gameObject.SetActive(true);
		pinsToPaste.SetActive(true);
		flyerPrice = 1;
		pinstToPasteText.text = "1";

	}

	public void PasteFlyer() {
		placingFlyer = false;
		pasteFlyerButton.gameObject.SetActive(false);
		addFlyerButton.gameObject.SetActive(true);
		pinsToPaste.SetActive(false);
		coins -= flyerPrice;
		moneyText.text = coins.ToString();
		flyersZOffset += 0.01f;
		currentFlyer.GetComponent<FlyerScript>().ForbidResize();
	}

	public void ChangeFlyerColor() {
		addFlyerBackgroundIndex = (addFlyerBackgroundIndex + 1) % addFlyerBackgrounds.Length;
		addFlyerUIBackground.sprite = addFlyerBackgrounds[addFlyerBackgroundIndex];
	}

}
