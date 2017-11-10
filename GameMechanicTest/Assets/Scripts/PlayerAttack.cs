using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

	private TurnObject c_myTurnObject;
	private PlayerHealth c_playerHealthScript;
	private bool c_checkForMove;

	private List<GameObject> c_squaresInMoveRange;
	private List<GameObject> c_squaresInAttackRange;
	private List<GameObject> c_enemiesInAttackRange;

	private PlayerMove c_playerMove;

	public bool c_myTurn;

	[SerializeField]
	private GameObject c_enemy;

	private PlayerHealth c_enemyHealthScript;

	private int c_personalDelay;

	[SerializeField]
	private UIControl c_UI;

	private string c_currentlyMoving;
	private bool c_playerMoved;

	private Vector3 c_saveStartPosition;
	private bool c_attacked;

	public ParticleSystem c_particleComponent;

	private bool c_finishedCheckRange;

	private bool c_setupAttack;

	private int c_personalDamageValue;

	[SerializeField]
	private string c_enemyDamageTag;

	private bool resetMove;

	// Use this for initialization
	void Start () {
		c_playerHealthScript = GetComponent<PlayerHealth> ();
		c_UI = GameObject.FindGameObjectWithTag ("UICanvas").GetComponent<UIControl>();
		c_personalDelay = 30 / c_playerHealthScript.c_playerStats.playerSpeed;
		c_myTurnObject = new TurnObject (gameObject, c_personalDelay);
		TurnOrder.s_turnOrderList.Add (c_myTurnObject);
		c_myTurnObject.c_myIndex = (TurnOrder.s_turnOrderList.Count - 1);
		if (c_myTurn) 
		{
			c_UI.UpdateActiveCharacter (gameObject);
		}
		if(c_enemy != null)
			c_enemyHealthScript = c_enemy.GetComponent<PlayerHealth> ();
		c_playerMove = GetComponent<PlayerMove> ();
		c_saveStartPosition = transform.position;

		c_squaresInMoveRange = new List<GameObject>();
		c_squaresInAttackRange = new List<GameObject> ();
		c_enemiesInAttackRange = new List<GameObject> ();
		c_attacked = false;
		c_setupAttack = false;
		resetMove = false;
		c_personalDamageValue = c_playerHealthScript.c_playerStats.playerStrength * 3;
		if(!c_myTurn)
			c_particleComponent.Stop();
	}
	
	// Update is called once per frame
	void Update () {
		if (c_myTurn && c_currentlyMoving != "Called Move") 
		{
			if (Input.GetKeyDown (KeyCode.W) && c_currentlyMoving != "Finished Moving") 
			{
				SetupMove ();
			}


//			else if (c_checkForMove && c_currentlyMoving != "Finished Moving" && c_finishedCheckRange == true) 
//			{
//				
//			}
			if(c_currentlyMoving == "Finished Moving")
			{
				c_UI.DynamicHide (true);
				if (Input.GetKeyDown (KeyCode.Backspace) || resetMove) 
				{
					resetMove = false;
					transform.position = c_saveStartPosition;
					c_currentlyMoving = "Restart Move";
					c_UI.UpdateUIMoved (false);
				}
			}
		}
	}

	public void ResetMovement(){
		resetMove = true;
	}

	public void Attack(){ //Called to start player turn
		c_myTurn = true;
		c_attacked = false;
		c_currentlyMoving = "Started Turn";
		c_UI.DynamicHide (true);
		c_UI.UpdateActiveCharacter (gameObject);
		c_saveStartPosition = transform.position;
		c_particleComponent.Play();
		Debug.Log ("" + gameObject.name + "'s turn");
	}

	public int DamageCalc(int l_baseDamage, float l_skillMult, float l_buffMult){
		int returnDamage = 0;
		returnDamage = (int)(((l_baseDamage) * l_skillMult) * l_buffMult);
		if (Random.Range (0, 85) < c_playerHealthScript.c_playerStats.playerSpeed) {
			returnDamage *= c_playerHealthScript.c_playerStats.playerSpeed;
		}
		return returnDamage;
	}

	public void MyAttack(){
		if (!c_setupAttack && !c_attacked) {
			c_setupAttack = true;
			c_squaresInAttackRange.Clear ();
			c_squaresInAttackRange = CheckRange (c_playerHealthScript.c_playerStats.playerAttackRange, "MoveCube");
			if (c_squaresInAttackRange.Count != 0) {
				foreach (GameObject tile in c_squaresInAttackRange) {
					Debug.Log ("Attempting to get character on tile...");
					if (tile.GetComponent<AddGridToRange> ().GetEnemyOnTile (c_enemyDamageTag) != null){
						Debug.Log (tile.GetComponent<AddGridToRange> ().GetEnemyOnTile (c_enemyDamageTag));
					}
					if (tile.GetComponent<AddGridToRange> ().GetEnemyOnTile (c_enemyDamageTag) != null) {
						c_enemiesInAttackRange.Add (tile.GetComponent<AddGridToRange> ().GetEnemyOnTile (c_enemyDamageTag));
						Debug.Log ("Added to tile");
					}
				}
			}
			if (c_enemiesInAttackRange.Count != 0) {
				c_UI.UpdateBattleDialogue ("Please select a target.");
				foreach (GameObject enemy in c_enemiesInAttackRange)
					Debug.Log (enemy.name);
				c_UI.DynamicHide (false);
				StartCoroutine (SearchForTile (c_squaresInAttackRange));
			} else {
				c_UI.UpdateBattleDialogue ("There are no targets in range.");
				c_setupAttack = false;
				foreach (GameObject tile in c_squaresInAttackRange) {
					tile.GetComponent<Renderer> ().material.SetColor ("_Color", Color.red);
				}
				Invoke ("DelayClearingSetupTiles", 1.5f);
			}
		}
		else if (c_enemy != null && !c_attacked) 
		{
			c_playerHealthScript.CancelDefend ();
			BattleDialogue l_sendDamage = new BattleDialogue (gameObject.name, DamageCalc(c_personalDamageValue, 1, 1));
			c_enemyHealthScript.TakeDamage (l_sendDamage);
			c_myTurnObject.c_delayValue += c_personalDelay;
			c_attacked = true;
			Debug.Log ("" + gameObject.name + " attacked");
			c_setupAttack = false;
			ClearTileColourInGrid (c_squaresInAttackRange);
			if (c_currentlyMoving == "Finished Moving") {
				EndTurn ();
			} else {
				c_UI.DynamicHide (true);
			}
		} 
		else if (c_attacked) 
		{
			c_UI.UpdateBattleDialogue ("You have already attacked/defended. Please move or end your turn.");
		}
		else if (c_enemy == null) 
		{
			c_UI.UpdateBattleDialogue ("Please select a new target.");
		} 
	}

	private void DelayClearingSetupTiles(){
		ClearTileColourInGrid (c_squaresInAttackRange);
	}

	public void MyDefend(){
		if (!c_attacked) 
		{
			c_playerHealthScript.Defend ();
			c_myTurnObject.c_delayValue += (int)(c_personalDelay*0.6f);
			c_attacked = true;
			c_UI.UpdateBattleDialogue ("" + gameObject.name + " defended.");
			Debug.Log ("" + gameObject.name + " defended");
			if (c_currentlyMoving == "Finished Moving") 
			{
				EndTurn ();
			}
		}
		else
		{
			c_UI.UpdateBattleDialogue ("You have already attacked/defended. Please move or end your turn.");
		}
	}

	public void SetupMove(){ //Called by button press on UI
		if (c_currentlyMoving != "Finished Moving" && c_currentlyMoving != "Called Move") 
		{
			c_checkForMove = true;
			c_UI.DynamicHide (false);
			c_UI.UpdateBattleDialogue ("Select position to move to with left click, cancel with right click.");
			c_squaresInMoveRange.Clear ();
			Debug.Log ("Calling CheckRange");
			c_squaresInMoveRange = CheckRange (c_playerHealthScript.c_playerStats.playerMoveRange, "MoveCube");
			Debug.Log ("Finished CheckRange");
			StartCoroutine (SearchForTile(c_squaresInMoveRange));
		} 
		else 
		{
			c_UI.UpdateBattleDialogue ("You have already moved. Please attack/defend or end your turn.");
		}
	}

	public void ClearTileColourInGrid(List<GameObject> l_tilesInRange){
		foreach (GameObject tile in l_tilesInRange) {
			tile.GetComponent<Renderer> ().material.SetColor ("_Color", Color.white);
			Debug.Log ("Trying to change colour of tiles");
		}
	}

	public IEnumerator SearchForTile(List<GameObject> l_listInRange){
		while (c_checkForMove) {
			RaycastHit l_hoverInfo = new RaycastHit ();
			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out l_hoverInfo)) {
				Debug.Log (l_hoverInfo.collider.gameObject.name);
				foreach (GameObject tile in l_listInRange) {
					tile.GetComponent<Renderer> ().material.SetColor ("_Color", Color.yellow);
				}
				if (l_hoverInfo.collider.CompareTag ("MoveCube") && l_listInRange.Contains (l_hoverInfo.collider.gameObject)) {
					l_hoverInfo.collider.GetComponent<Renderer> ().material.SetColor ("_Color", Color.green);
				}
				if (Input.GetMouseButtonDown (0)) {
					Vector3 l_movePos = new Vector3 (0, 0, 0);
					RaycastHit hitInfo = new RaycastHit ();
					bool hit = Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hitInfo);
					if (hit) {
						if (hitInfo.transform.gameObject.tag == "MoveCube" && l_listInRange.Contains (hitInfo.collider.gameObject)) {
							l_movePos = hitInfo.transform.position;
							c_UI.UpdateBattleDialogue ("Moving...");
							Debug.Log ("Selected movement square");
							ClearTileColourInGrid (c_squaresInMoveRange);
							MyMove (l_movePos);
							c_checkForMove = false;
						}
					}
				} else if (Input.GetMouseButtonDown (1)) {
					CancelMove ();
				}
			}
			yield return null;
		}
		while (c_setupAttack) {
			RaycastHit l_hoverInfo = new RaycastHit ();
			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out l_hoverInfo)) {
				Debug.Log (l_hoverInfo.collider.gameObject.name);
				foreach (GameObject tile in l_listInRange) {
					tile.GetComponent<Renderer> ().material.SetColor ("_Color", Color.yellow);
				}
				if (l_hoverInfo.collider.CompareTag ("MoveCube") && l_listInRange.Contains (l_hoverInfo.collider.gameObject)) {
					l_hoverInfo.collider.GetComponent<Renderer> ().material.SetColor ("_Color", Color.green);
				}

				if (Input.GetMouseButtonDown (0)) {
					RaycastHit hitInfo = new RaycastHit ();
					bool hit = Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hitInfo);
					if (hit) {
						if (hitInfo.transform.gameObject.tag == c_enemyDamageTag && c_enemiesInAttackRange.Contains (hitInfo.collider.gameObject)) {
							c_enemy = hitInfo.transform.gameObject;
							c_UI.UpdateBattleDialogue ("" + gameObject.name + " is now targeting " + c_enemy.name + ".");
							Debug.Log ("Selected new enemy");
							c_enemyHealthScript = c_enemy.GetComponent<PlayerHealth> ();
							MyAttack ();
						}
					}
				} else if (Input.GetMouseButtonDown (1)) {
					c_setupAttack = false;
					c_UI.DynamicHide (true);
					ClearTileColourInGrid (c_squaresInAttackRange);
					c_squaresInAttackRange.Clear ();
					c_enemiesInAttackRange.Clear ();
				}
			}
			yield return null;
		}
		Debug.Log ("Finishing SearchForTile");
		StopCoroutine ("SearchForTile");
	}

	public List<GameObject> CheckRange(int rangeValue, string l_tagToCompare){
		List<RaycastHit> l_inRange = new List<RaycastHit>();
		List<GameObject> l_returnList = new List<GameObject> ();
		for (int z = rangeValue; z >= 0; z--) {
			for (int x = 0; x <= rangeValue; x++) {
				if (x + z == rangeValue) {
					//Debug.DrawRay (transform.position - new Vector3 (x * 10, -1f, (-z - 0.5f) * 10), -(Vector3.forward * ((z * 2) + 1)) * 10, Color.red, 15f);
					l_inRange.AddRange(Physics.RaycastAll(transform.position - new Vector3 (x * 10, -1f, (-z - 0.5f) * 10), -Vector3.forward, ((z * 2) + 0.5f) * 10));
					if (x != 0) {
						//Debug.DrawRay (transform.position - new Vector3 (-x * 10, -1f, (-z - 0.5f) * 10), -(Vector3.forward * ((z * 2) + 1)) * 10, Color.red, 15f);
						l_inRange.AddRange (Physics.RaycastAll (transform.position - new Vector3 (-x * 10, -1f, (-z - 0.5f) * 10), -Vector3.forward, ((z * 2) + 0.5f) * 10));
					}
				}
			}
		}
		foreach (RaycastHit hit in l_inRange) {
			if(hit.collider.CompareTag(l_tagToCompare))
				l_returnList.Add (hit.collider.gameObject);
		}

		return l_returnList;
	}

	public void CancelMove(){
		c_checkForMove = false;
		c_UI.DynamicHide (true);
		ClearTileColourInGrid (c_squaresInMoveRange);
		c_squaresInMoveRange.Clear ();
		c_UI.UpdateBattleDialogue ("Move Cancelled.");
	}

	public void MyMove(Vector3 l_target){
		if (c_currentlyMoving != "Finished Moving" && c_currentlyMoving != "Called Move") 
		{
			Debug.Log ("Calling Move with target");
			Debug.Log (l_target);
			c_UI.UpdateBattleDialogue ("Moving...");
			c_currentlyMoving = "Called Move";
			c_currentlyMoving = c_playerMove.InitiateMove (transform.position, l_target);
			c_myTurnObject.c_delayValue += (int)(c_personalDelay * 0.7);
			c_UI.UpdateUIMoved (true);
			ClearTileColourInGrid (c_squaresInMoveRange);
			if (c_attacked) 
			{
				Debug.Log ("Turn over.");
				EndTurn ();
			}
		} 
		else 
		{
			c_UI.UpdateBattleDialogue ("You have already moved. Please attack/defend or end your turn.");
		}
	}

	public void EndTurn(){
		c_UI.UpdateBattleDialogue ("Turn over.");
		c_myTurn = false;
		c_UI.DynamicHide (false);
		c_particleComponent.Stop();
		c_myTurnObject.c_delayValue += (int)(c_personalDelay * 0.5);
		ClearTileColourInGrid (c_squaresInAttackRange);
		ClearTileColourInGrid (c_squaresInMoveRange);
		c_UI.UpdateUIMoved (false);
		c_enemiesInAttackRange.Clear ();
		Invoke ("DelayCallNext", 1.5f);
	}

	private void DelayCallNext(){
		
		TurnOrder.CallNextTurn ();
	}

	public void DeathDelay(){
		c_myTurnObject.c_delayValue += 1000;
	}
}
