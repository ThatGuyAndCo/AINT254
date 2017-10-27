using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDamageTest: MonoBehaviour
{
	[SerializeField]
	private GameObject c_floatingTextPrefab;

	[SerializeField]
	private GameObject c_floatingTextCanvas;

	void Start(){
		c_floatingTextCanvas = GameObject.FindGameObjectWithTag ("FloatingCanvas");
	}

	public void SetText(string l_textToDisplay, Color l_colourToDisplay, GameObject l_sender){
		GameObject l_floater = Instantiate (c_floatingTextPrefab);
		l_floater.GetComponent<FloatingTextTracker> ().setCharacterToFloatOver(l_sender);
		l_floater.GetComponent<FloatingTextTracker> ().SetTextAndColour(l_textToDisplay, l_colourToDisplay);
		l_floater.transform.SetParent (c_floatingTextCanvas.transform);
	}

}

