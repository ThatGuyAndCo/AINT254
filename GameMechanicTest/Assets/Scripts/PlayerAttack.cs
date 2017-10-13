using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

	private TurnObject c_myTurnObject;
	private PlayerHealth c_playerHealthScript;

	[SerializeField]
	private bool c_myTurn;

	[SerializeField]
	private GameObject c_enemy;

	private EnemyHealth c_enemyHealthScript;

	[SerializeField]
	private int c_personalDelay;

	[SerializeField]
	private UIControl c_UI;

	// Use this for initialization
	void Start () {
		c_myTurnObject = new TurnObject (gameObject, c_personalDelay);
		TurnOrder.s_turnOrderList.Add (c_myTurnObject);
		c_myTurnObject.c_myIndex = (TurnOrder.s_turnOrderList.Count - 1);
		if (c_myTurn) {
			c_UI.UpdateActiveCharacter (gameObject);
		}
		c_playerHealthScript = GetComponent<PlayerHealth> ();
		c_enemyHealthScript = c_enemy.GetComponent<EnemyHealth> ();
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
						c_UI.UpdateBattleDialogue ("" + gameObject.name + " is now targeting " + c_enemy.name + ".");
						Debug.Log ("Selected new enemy");
						c_enemyHealthScript = c_enemy.GetComponent<EnemyHealth> ();
					}
				}
			}

			if (Input.GetKeyDown (KeyCode.A)) {
				MyAttack ();
			} else if (Input.GetKeyDown (KeyCode.D)) {
				MyDefend ();
			}
		}
	}

	public void Attack(){
		c_myTurn = true;
		c_UI.DynamicHide (true);
		c_UI.UpdateActiveCharacter (gameObject);
		Debug.Log ("" + gameObject.name + "'s turn");
	}

	public void MyAttack(){
		if (c_enemy != null) {
			c_playerHealthScript.CancelDefend ();
			BattleDialogue l_sendDamage = new BattleDialogue (gameObject.name, 20);
			c_enemyHealthScript.TakeDamage (l_sendDamage);
			c_myTurnObject.c_delayValue += 25;
			c_myTurn = false;
			Debug.Log ("" + gameObject.name + " attacked");
			c_UI.DynamicHide (false);
			Invoke("DelayCallNext", 1.5f);
		} else {
			Debug.Log ("Please select a new target.");
		}
	}

	public void MyDefend(){
		gameObject.SendMessage ("Defend");
		c_myTurnObject.c_delayValue += 15;
		c_myTurn = false;
		c_UI.UpdateBattleDialogue ("" + gameObject.name + " defended.");
		Debug.Log ("" + gameObject.name + " defended");
		c_UI.DynamicHide (false);
		Invoke("DelayCallNext", 1.5f);
	}

	private void DelayCallNext(){
		
		TurnOrder.CallNextTurn ();
	}

	public void DeathDelay(){
		c_myTurnObject.c_delayValue += 1000;
	}
}
