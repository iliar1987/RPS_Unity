using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPSMouseInteraction : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	const float fRadius = 0.02f;
	const float fStrengthFactor = 2.5f;
	Color colCurrent = Color.green;
	enum EColorState {r,g,b};
	EColorState eColState=EColorState.g;
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Joystick1Button0)
			|| Input.GetKey (KeyCode.Joystick1Button1)
			||Input.GetKey (KeyCode.Joystick1Button2)) {
			var cam = Camera.main.transform;
			RaycastHit hit;
			if (Physics.Raycast (cam.position, cam.forward, out hit, 1000)) {
				Color col=Color.black;
				if (Input.GetKey (KeyCode.Joystick1Button0)) {
					col +=Color.green;
				}
				if (Input.GetKey (KeyCode.Joystick1Button2)) {
					col += Color.blue;
				}
				if (Input.GetKey (KeyCode.Joystick1Button1)) {
					col += Color.red;
				}
				GameObject.Find ("RPSGameObject").GetComponent<UpdateRPS> ().AddColor 
				(hit.textureCoord, 
					col, 
					fRadius,
					(float)(Time.deltaTime * fStrengthFactor)
				);
			}
		}
		if (Input.GetMouseButton (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			//ray.origin = ray.GetPoint (1000);
			//ray.direction = -ray.direction;
			RaycastHit hit;

			if (GetComponent<Collider> ().Raycast (ray, out hit, 1000)) {				//we hit
				GameObject.Find ("RPSGameObject").GetComponent<UpdateRPS> ().AddColor 
					(hit.textureCoord, 
					colCurrent, 
					fRadius,
					(float)(Time.deltaTime * fStrengthFactor)
				);
			}
		}
		int dirChange = 0;
		var d = Input.GetAxis("Mouse ScrollWheel");
		if (d > 0) {
			dirChange = 1;
		} else if (d < 0) {
			dirChange = -1;
		}

		if (dirChange != 0) {
			eColState += dirChange;
			if (eColState > EColorState.b)
				eColState = EColorState.r;
			else if (eColState < 0) {
				eColState = EColorState.b;
			}
			switch (eColState) {
			case EColorState.r:
				eColState = EColorState.r;
				colCurrent = Color.red;
				break;
			case EColorState.b:
				eColState = EColorState.b;
				colCurrent = Color.blue;
				break;
			case EColorState.g:
				eColState = EColorState.g;
				colCurrent = Color.green;
				break;
			}
		}
	}
}
