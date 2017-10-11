using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

	private TurnObject c_myTurnObject;
	private GameObject[] c_targets;

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
	}

	public void Attack(){
		c_targets = GameObject.FindGameObjectsWithTag ("PlayerTeam");
		GameObject l_target;
		int l_randomTarget = Random.Range (0, c_targets.Length);
		l_target = c_targets [l_randomTarget];
		l_target.SendMessage ("TakeDamage", 20);
		c_myTurnObject.c_delayValue += 30;
		Debug.Log ("Enemy attacked");
		Invoke("DelayCallNext", 0.5f);
	}

	private void DelayCallNext(){
		TurnOrder.CallNextTurn();
	}

	public void DeathDelay(){
		c_myTurnObject.c_delayValue += 1000;
	}
}
