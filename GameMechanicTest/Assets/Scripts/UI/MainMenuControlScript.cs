using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuControlScript : MonoBehaviour {

	private int c_instPageNum;

	[SerializeField]
	private GameObject c_instGroup;

	[SerializeField]
	private GameObject c_mainGroup;

	[SerializeField]
	private GameObject c_instPg1;

	[SerializeField]
	private GameObject c_instPg2;

	void Start(){
		c_instPageNum = 1;
	}

	public void StartGame(){
		SceneManager.LoadScene (1);
	}

	public void InstructionScreen(){
		c_mainGroup.SetActive (!c_mainGroup.activeSelf);
		c_instGroup.SetActive (!c_instGroup.activeSelf);
	}

	public void InstBackButton(bool next){
		if (next)
			c_instPageNum++;
		else
			c_instPageNum--;

		if (c_instPageNum == 1) {//Teams
			c_instPg1.SetActive(true);
			c_instPg2.SetActive (false);
		} else if (c_instPageNum == 2) {//Controls
			c_instPg1.SetActive(false);
			c_instPg2.SetActive (true);
		} else if (c_instPageNum == 0) {//Close Inst Screen
			InstructionScreen ();
			c_instPageNum = 1;
		}
	}

//	public void SetupGame(){
//
//	}

	public void ExitGame(){
		Application.Quit ();
	}
}
