using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

	private TurnObject c_myTurnObject;
	private GameObject[] c_targets;
	private UIControl c_UI;

	private PlayerHealth c_playerHealthScript;

	[SerializeField]
	private int c_personalDelay;

	// Use this for initialization
	void Start () {
		c_myTurnObject = new TurnObject (gameObject, c_personalDelay);
		TurnOrder.s_turnOrderList.Add (c_myTurnObject);
		c_myTurnObject.c_myIndex = (TurnOrder.s_turnOrderList.Count - 1);
		c_UI = GameObject.FindGameObjectWithTag ("UICanvas").GetComponent<UIControl> ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Attack(){
		c_UI.UpdateCamera (gameObject);
		c_targets = GameObject.FindGameObjectsWithTag ("PlayerTeam");
		GameObject l_target;
		int l_randomTarget = Random.Range (0, c_targets.Length);
		l_target = c_targets [l_randomTarget];
		c_playerHealthScript = l_target.GetComponent<PlayerHealth> ();
		BattleDialogue l_sendDamage = new BattleDialogue (gameObject.name, 15);
		c_playerHealthScript.TakeDamage (l_sendDamage);
		c_myTurnObject.c_delayValue += 20;
		Debug.Log ("Enemy attacked");
		Invoke("DelayCallNext", 1.5f);
	}

	private void DelayCallNext(){
		TurnOrder.CallNextTurn();
	}

	public void DeathDelay(){
		c_myTurnObject.c_delayValue += 1000;
	}
}
