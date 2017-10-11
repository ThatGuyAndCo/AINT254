using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

	private TurnObject c_myTurnObject;

	[SerializeField]
	private bool c_myTurn;

	[SerializeField]
	private GameObject c_enemy;

	[SerializeField]
	private int c_personalDelay;

	// Use this for initialization
	void Start () {
		c_myTurnObject = new TurnObject (gameObject, c_personalDelay);
		TurnOrder.s_turnOrderList.Add (c_myTurnObject);
		c_myTurnObject.c_myIndex = (TurnOrder.s_turnOrderList.Count - 1);
	}
	
	// Update is called once per frame
	void Update () {
		if (c_myTurn == true){

			if (Input.GetMouseButtonDown (0)) {
				RaycastHit hitInfo = new RaycastHit ();
				bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
				if (hit) {
					if (hitInfo.transform.gameObject.tag == "EnemyTeam") {
						c_enemy = hitInfo.transform.gameObject;
						Debug.Log ("Selected new enemy");
					}
				}
			}

			if (Input.GetKeyDown (KeyCode.A)) {
				gameObject.SendMessage ("CancelDefend");
				MyAttack ();
			} else if (Input.GetKeyDown (KeyCode.D)) {
				MyDefend ();
			}
		}
	}

	public void Attack(){
		c_myTurn = true;
		Debug.Log ("" + gameObject.name + "'s turn");
	}

	public void MyAttack(){
		if (c_enemy != null) {
			c_enemy.SendMessage ("TakeDamage", 20);
			c_myTurnObject.c_delayValue += 25;
			c_myTurn = false;
			Debug.Log ("" + gameObject.name + " attacked");
			Invoke("DelayCallNext", 0.5f);
		} else {
			Debug.Log ("Please select a new target.");
		}
	}

	public void MyDefend(){
		gameObject.SendMessage ("Defend");
		c_myTurnObject.c_delayValue += 15;
		c_myTurn = false;
		Debug.Log ("" + gameObject.name + " defended");
		TurnOrder.CallNextTurn ();
	}

	private void DelayCallNext(){
		TurnOrder.CallNextTurn ();
	}

	public void DeathDelay(){
		c_myTurnObject.c_delayValue += 1000;
	}
}
