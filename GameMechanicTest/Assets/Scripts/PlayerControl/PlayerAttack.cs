using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class PlayerAttack : MonoBehaviour {

	private TurnObject c_myTurnObject;
	private PlayerHealth c_playerHealthScript;
	private bool c_checkForMove;

	private List<GameObject> c_squaresInMoveRange;
	private List<GameObject> c_squaresInAttackRange;
	private List<GameObject> c_enemiesInAttackRange;

	private PlayerMoveAStar c_playerMove;
	private PlayerMoveRangeDijkstra c_playerMoveRangeScript;

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

	public string c_teamDamageTag;
	public string c_enemyDamageTag;

	public string c_myClass;

	private string c_tagForSkillToUse;

	[SerializeField]
	private List<AbstractSkill> c_characterSkills;

	private AbstractSkill c_currentSkill;

	private bool c_resetMove;

	private int saveAttackRange;


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
		c_playerMove = GetComponent<PlayerMoveAStar> ();
		c_playerMoveRangeScript = GetComponent<PlayerMoveRangeDijkstra> ();
		c_saveStartPosition = transform.position;
		c_squaresInMoveRange = new List<GameObject>();
		c_squaresInAttackRange = new List<GameObject> ();
		c_enemiesInAttackRange = new List<GameObject> ();
		c_attacked = false;
		c_setupAttack = false;
		c_resetMove = false;
		saveAttackRange = c_playerHealthScript.c_playerStats.playerAttackRange;
		SetSkill("BasicAttack");
		if(!c_myTurn)
			c_particleComponent.Stop();
	}

	public void SetSkill(string l_skillName){
		switch(l_skillName){
		case "BasicAttack":
			c_currentSkill = new BasicAttack ();
			c_playerHealthScript.c_playerStats.playerAttackRange = saveAttackRange;
			c_tagForSkillToUse = c_enemyDamageTag;
			Debug.Log ("Setting skill to " + l_skillName);
			break;
		case "Warmth":
			c_currentSkill = new Warmth ();
			c_playerHealthScript.c_playerStats.playerAttackRange = c_currentSkill.getPlayerSkillRange();
			Debug.Log ("Player range = " + c_playerHealthScript.c_playerStats.playerAttackRange + ", skill range = " + c_currentSkill.getPlayerSkillRange ());
			c_tagForSkillToUse = c_teamDamageTag;
			Debug.Log ("Setting skill to " + l_skillName);
			break;
		case "Flare":
			c_currentSkill = new FlareSkill ();
			c_playerHealthScript.c_playerStats.playerAttackRange = c_currentSkill.getPlayerSkillRange();
			Debug.Log ("Player range = " + c_playerHealthScript.c_playerStats.playerAttackRange + ", skill range = " + c_currentSkill.getPlayerSkillRange ());
			c_tagForSkillToUse = c_enemyDamageTag;
			Debug.Log ("Setting skill to " + l_skillName);
			break;
		case "SoothingRiver":
			c_currentSkill = new SoothingRiver ();
			c_playerHealthScript.c_playerStats.playerAttackRange = c_currentSkill.getPlayerSkillRange();
			Debug.Log ("Player range = " + c_playerHealthScript.c_playerStats.playerAttackRange + ", skill range = " + c_currentSkill.getPlayerSkillRange ());
			c_tagForSkillToUse = c_teamDamageTag;
			Debug.Log ("Setting skill to " + l_skillName);
			break;
		case "TidalSurge":
			c_currentSkill = new TidalSurge ();
			c_playerHealthScript.c_playerStats.playerAttackRange = c_currentSkill.getPlayerSkillRange();
			Debug.Log ("Player range = " + c_playerHealthScript.c_playerStats.playerAttackRange + ", skill range = " + c_currentSkill.getPlayerSkillRange ());
			c_tagForSkillToUse = c_teamDamageTag;
			Debug.Log ("Setting skill to " + l_skillName);
			break;
		case "ArmBreaker":
			c_currentSkill = new ArmBreaker ();
			c_playerHealthScript.c_playerStats.playerAttackRange = c_currentSkill.getPlayerSkillRange();
			Debug.Log ("Player range = " + c_playerHealthScript.c_playerStats.playerAttackRange + ", skill range = " + c_currentSkill.getPlayerSkillRange ());
			c_tagForSkillToUse = c_enemyDamageTag;
			Debug.Log ("Setting skill to " + l_skillName);
			break;
		case "WarCry":
			c_currentSkill = new WarCry ();
			c_playerHealthScript.c_playerStats.playerAttackRange = c_currentSkill.getPlayerSkillRange();
			Debug.Log ("Player range = " + c_playerHealthScript.c_playerStats.playerAttackRange + ", skill range = " + c_currentSkill.getPlayerSkillRange ());
			c_tagForSkillToUse = c_teamDamageTag;
			Debug.Log ("Setting skill to " + l_skillName);
			break;
		case "Pounce":
			c_currentSkill = new Pounce ();
			c_playerHealthScript.c_playerStats.playerAttackRange = c_currentSkill.getPlayerSkillRange ();
			Debug.Log ("Player range = " + c_playerHealthScript.c_playerStats.playerAttackRange + ", skill range = " + c_currentSkill.getPlayerSkillRange ());
			c_tagForSkillToUse = c_enemyDamageTag;
			Debug.Log ("Setting skill to " + l_skillName);
			break;
		case "SwiftStrike":
			c_currentSkill = new SwiftStrike ();
			c_playerHealthScript.c_playerStats.playerAttackRange = c_currentSkill.getPlayerSkillRange();
			Debug.Log ("Player range = " + c_playerHealthScript.c_playerStats.playerAttackRange + ", skill range = " + c_currentSkill.getPlayerSkillRange ());
			c_tagForSkillToUse = c_enemyDamageTag;
			Debug.Log ("Setting skill to " + l_skillName);
			break;
		case "Stalwart":
			c_currentSkill = new Stalwart ();
			c_playerHealthScript.c_playerStats.playerAttackRange = c_currentSkill.getPlayerSkillRange();
			Debug.Log ("Player range = " + c_playerHealthScript.c_playerStats.playerAttackRange + ", skill range = " + c_currentSkill.getPlayerSkillRange ());
			c_tagForSkillToUse = c_teamDamageTag;
			Debug.Log ("Setting skill to " + l_skillName);
			break;
		case "Crunch":
			c_currentSkill = new Crunch ();
			c_playerHealthScript.c_playerStats.playerAttackRange = c_currentSkill.getPlayerSkillRange();
			Debug.Log ("Player range = " + c_playerHealthScript.c_playerStats.playerAttackRange + ", skill range = " + c_currentSkill.getPlayerSkillRange ());
			c_tagForSkillToUse = c_enemyDamageTag;
			Debug.Log ("Setting skill to " + l_skillName);
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (c_myTurn && c_currentlyMoving != "Called Move") 
		{
			if (Input.GetKeyDown (KeyCode.W) && c_currentlyMoving != "Finished Moving") 
			{
				SetupMove ();
			}
			if (Input.GetKeyDown (KeyCode.H)) {
				c_UI.UpdateBattleDialogue ("Healing skill selected");
				SetSkill ("HealingAttack");
			}
			if (Input.GetKeyDown (KeyCode.B)) {
				c_UI.UpdateBattleDialogue ("Basic skill selected");
				SetSkill ("BasicAttack");
			}


//			else if (c_checkForMove && c_currentlyMoving != "Finished Moving" && c_finishedCheckRange == true) 
//			{
//				
//			}
			if(c_currentlyMoving == "Finished Moving" && !c_setupAttack)
			{
				c_UI.DynamicHide (true);
				if (Input.GetKeyDown (KeyCode.Backspace) || c_resetMove) 
				{
					c_resetMove = false;
					transform.position = c_saveStartPosition;
					c_currentlyMoving = "Restart Move";
					c_UI.UpdateUIMoved (false);
				}
			}
		}
	}

	public void ResetMovement(){
		c_resetMove = true;
	}

	public void Attack(){ //Called to start player turn
		c_myTurn = true;
		c_attacked = false;
		c_setupAttack = false;
		c_enemy = null;
		c_currentlyMoving = "Started Turn";
		c_UI.DynamicHide (true);
		c_UI.UpdateActiveCharacter (gameObject);
		c_saveStartPosition = transform.position;
		c_particleComponent.Play();
		c_playerHealthScript.CheckStatusEff ();
		Debug.Log ("" + gameObject.name + "'s turn");
	}

	public void MyAttack(){
		Debug.Log (!c_setupAttack + ", " + !c_attacked);
		if (!c_setupAttack && !c_attacked) {
			c_setupAttack = true;
			c_UI.DynamicHide (false);
			c_squaresInAttackRange.Clear ();
			c_squaresInAttackRange = GridTest.CheckRange (transform.position, c_playerHealthScript.c_playerStats.playerAttackRange, "MoveCube");
			if (c_squaresInAttackRange.Count != 0) {
				foreach (GameObject tile in c_squaresInAttackRange) {
					//Debug.Log ("Attempting to get character on tile...");
					if (tile.GetComponent<AddGridToRange> ().GetEnemyOnTile (c_tagForSkillToUse) != null) {
						c_enemiesInAttackRange.Add (tile.GetComponent<AddGridToRange> ().GetEnemyOnTile (c_tagForSkillToUse));
						//Debug.Log ("Added to tile");
					}
				}
			}
			if (c_enemiesInAttackRange.Count != 0) {
				c_UI.UpdateBattleDialogue ("Please select a target.");
				Debug.Log (c_playerHealthScript.c_playerStats.playerAttackRange);
				StartCoroutine (SearchForTile (c_squaresInAttackRange));
			} else {
				c_UI.UpdateBattleDialogue ("There are no targets in range.");
				c_setupAttack = false;
				c_UI.DynamicHide (true);
				foreach (GameObject tile in c_squaresInAttackRange) {
					tile.GetComponent<Renderer> ().material.SetColor ("_Color", Color.red);
				}
				Invoke ("DelayClearingSetupTiles", 1.5f);
			}
		}
		else if (c_enemy != null && !c_attacked) 
		{
			c_playerHealthScript.CancelDefend ();
			c_myTurnObject.c_delayValue += (int)(c_personalDelay * c_currentSkill.UseSkill (c_enemy.transform.position, c_playerHealthScript, c_tagForSkillToUse));
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
			Debug.Log ("c_enemy == null statement entered");
			//c_UI.UpdateBattleDialogue ("Please select a new target.");
		} 
	}

	public void SetCurrentSkill(int l_skillNum){
		c_currentSkill = c_characterSkills [l_skillNum];
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
			c_squaresInMoveRange = new List<GameObject>(c_playerMoveRangeScript.FindMoveArea (transform.position, c_playerHealthScript.c_playerStats.playerMoveRange));
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
			//Debug.Log ("Trying to change colour of tiles");
		}
	}

	public IEnumerator SearchForTile(List<GameObject> l_listInRange){
		while (c_checkForMove) {
			RaycastHit l_hoverInfo = new RaycastHit ();
			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out l_hoverInfo)) {
				//Debug.Log (l_hoverInfo.collider.gameObject.name);
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
                        if (hitInfo.transform.gameObject.tag == "MoveCube" && l_listInRange.Contains(hitInfo.collider.gameObject) && hitInfo.collider.GetComponent<AddGridToRange>().GetEnemyOnTile("") == null) {
							l_movePos = hitInfo.transform.position;
							c_UI.UpdateBattleDialogue ("Moving...");
							//Debug.Log ("Selected movement square");
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
				//Debug.Log (l_hoverInfo.collider.gameObject.name);
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
						if (hitInfo.transform.gameObject.tag == c_tagForSkillToUse && c_enemiesInAttackRange.Contains (hitInfo.collider.gameObject)) {
							c_enemy = hitInfo.transform.gameObject;
							c_UI.UpdateBattleDialogue ("" + gameObject.name + " is now targeting " + c_enemy.name + ".");
							//Debug.Log ("Selected new enemy");
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

		Debug.Log ("Setup attack = " + c_setupAttack);
		Debug.Log ("Finishing SearchForTile");
		StopCoroutine ("SearchForTile");
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
