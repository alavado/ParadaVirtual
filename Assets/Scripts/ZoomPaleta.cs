using UnityEngine;
using System.Collections;

public class ZoomPaleta : MonoBehaviour {

	private Transform target;
	public float smoothTime = 2.0f;
	private Vector3 velocity = new Vector3(30.0f, 3.0f, 3.0f);

	float smooth = 5.0f;
	float tilt = -180.0f;
	bool girar = false;
	bool volver = false;

	public float posx_zoom = 0.0f;
	public float posy_zoom = 24.0f;
	public float posz_zoom = 12.5f;

	public float def_x = 35.2f, def_y = 20.0f, def_z = 29.4f;

	public float def_rot_x = 0.0f, def_rot_y = -90.0f, def_rot_z = 0.0f;

	private bool showGUI = false;

	// Use this for initialization
	void Start () {
		target = Camera.main.transform;
	}

	// Update is called once per frame
	void Update () {
		if(StarScript.isActive) return;
		if (girar) {
			showGUI = true;
			Quaternion rot = Quaternion.Euler (0.0f, -180.0f, 0.0f);
			target.rotation = Quaternion.Slerp (target.rotation, rot, Time.deltaTime * smooth);
			Vector3 targetPosition = new Vector3 (posx_zoom, posy_zoom, posz_zoom);
			target.position = Vector3.Slerp (target.position, targetPosition, Time.deltaTime * smooth);
		} else if (volver) {
			showGUI = false;
			Quaternion rot = Quaternion.Euler (def_rot_x, def_rot_y, def_rot_z);
			target.rotation = Quaternion.Slerp (target.rotation, rot, Time.deltaTime * smooth);
			Vector3 targetPosition = new Vector3 (def_x, def_y, def_z);
			target.position = Vector3.Slerp (target.position, targetPosition, Time.deltaTime * smooth);
			if (Mathf.Abs (target.position.x - def_x) < 0.5) {
				volver = false;
			}		
		}
	}

	void OnGUI() {
		if (showGUI) {
			if (GUI.Button (new Rect (10, 10, 100, 100), "Volver")) {
				showGUI = false;
				volver = true;
				girar = false;
			}
		}
	}

	void OnMouseDown() {
		girar = true;
	}
}
