using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard1 : MonoBehaviour {

	Material mat;
	UpdateRPS rpsScript;
	// Use this for initialization
	void Start () {
		mat = gameObject.GetComponent<Renderer>().materials [0];
		rpsScript = GameObject.Find ("RPSGameObject").GetComponent<UpdateRPS> ();
	}
	
	// Update is called once per frame
	void Update () {
//		var camera = Camera.main;
//		transform.forward = camera.transform.forward;
//		transform.position = camera.transform.position + camera.transform.forward*10;
		mat.mainTexture = rpsScript.GetCurrentFrameTexture();

	}
}
