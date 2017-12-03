using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextTracker : MonoBehaviour {

	private GameObject c_characterToFloatOver;

	private Text c_textToDisplay;

	private Camera c_mainCamera;

	[SerializeField]
	private float c_floatingTextOffset;

	private RectTransform c_floatingTextPosition;

	// Use this for initialization
	void Start () {
		c_floatingTextPosition = GetComponent<RectTransform> ();
		c_textToDisplay = GetComponent<Text> ();
		c_mainCamera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (c_characterToFloatOver != null) {
			Vector3 l_position = RectTransformUtility.WorldToScreenPoint (c_mainCamera, c_characterToFloatOver.transform.position);
			l_position.y += c_floatingTextOffset;
			c_floatingTextPosition.position = l_position;
		}
		c_textToDisplay.color = Color.Lerp (c_textToDisplay.color, Color.clear, 0.01f);
		c_floatingTextOffset += 0.3f;
		if (c_textToDisplay.color == Color.clear) {
			Destroy (gameObject);
		}
	}

	public void SetTextAndColour(string l_textToDisplay, Color l_colourToDisplay){
		c_textToDisplay = GetComponent<Text> ();
		c_textToDisplay.text = l_textToDisplay;
		c_textToDisplay.color = l_colourToDisplay;
	}
		
	public void setCharacterToFloatOver(GameObject l_characterToFloatOver){
		c_characterToFloatOver = l_characterToFloatOver;
	}
}
