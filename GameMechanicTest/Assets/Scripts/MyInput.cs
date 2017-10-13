using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyInput : MonoBehaviour {

	private void OnGUI(){
		GUI.Label (new Rect (5.0f, 10.0f, 100.0f, 20.0f), "Horizontal Axis: ");
		GUI.Label (new Rect (150.0f, 10.0f, 100.0f, 20.0f),"" + Input.GetAxis("Horizontal"));
		GUI.Label (new Rect (5.0f, 30.0f, 100.0f, 20.0f), "Vertical Axis: ");
		GUI.Label (new Rect (150.0f, 30.0f, 100.0f, 20.0f),"" + Input.GetAxis("Vertical"));
	}
}
