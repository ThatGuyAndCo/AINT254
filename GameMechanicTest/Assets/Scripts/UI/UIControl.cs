using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIControl: MonoBehaviour
{

	[SerializeField]
	private Text c_characterName;

	public GameObject c_activeCharacter;

	private PlayerAttack c_playerAttackScript;

	[SerializeField] 
	private GameObject c_displayContainer;

	[SerializeField]
	private Text c_battleDialogue;

	[SerializeField]
	private Vector3 c_cameraRotation;

	[SerializeField]
	private Vector3 c_cameraPositionOffset;

	[SerializeField]
	private Camera c_main;

	[SerializeField]
	private GameObject c_movementButton;

	[SerializeField]
	private GameObject c_resetMoveButton;

	[SerializeField]
	private GameObject c_skillContainer;

	[SerializeField]
	private GameObject[] c_skillButtons;

	[SerializeField]
	private Image c_battleLog;

	private FloatingDamageTest c_floatingTextTest;

	[SerializeField]
	private GameObject c_skillDescCont;

	[SerializeField]
	private Text[] c_skillTextComps;

	void Start(){
		c_battleDialogue.text = "";
		c_floatingTextTest = GetComponent<FloatingDamageTest> ();
	}

	public void UpdateActiveCharacter(GameObject l_activeCharacter){
		c_activeCharacter = l_activeCharacter;
		c_playerAttackScript = c_activeCharacter.GetComponent<PlayerAttack> ();
		c_characterName.text = "" + l_activeCharacter.name + "'s turn";
		UpdateCamera (l_activeCharacter);
		if(c_activeCharacter.CompareTag("PlayerTeam")){
			ChangeBattleLogColour(new Color(0.2f, 0.5f, 0.7f, 0.7f));
		}else{
			ChangeBattleLogColour(new Color(0.6f, 0, 0, 0.7f));
		}
	}

	private void ChangeBattleLogColour(Color l_teamColour){
		c_battleLog.color = l_teamColour;
	}

	public void DynamicHide(bool l_hide){ //true = display, false = hidden
		c_displayContainer.SetActive(l_hide);
	}

	public void AttackButtonClicked(){
		ActivateSkill ("BasicAttack");
		c_playerAttackScript.MyAttack ();
		c_skillContainer.SetActive (false);
	}

	public void SkillButtonClicked(){
		c_skillContainer.SetActive (!c_skillContainer.activeSelf);
		SetupSkill ();
	}

	public void SetupSkill(){ 
		c_skillButtons [0].GetComponent<Button> ().onClick.RemoveAllListeners();
		c_skillButtons [1].GetComponent<Button> ().onClick.RemoveAllListeners();
		switch (c_playerAttackScript.c_myClass) {
		case "BlueMage":
			c_skillButtons [0].GetComponentInChildren<Text> ().text = "Soothing River";
			c_skillButtons [0].GetComponent<Button> ().onClick.AddListener(delegate{ActivateSkill ("SoothingRiver");});
			Debug.Log ("Blue Mage set skill 1 to Soothing River");
			c_skillButtons [1].GetComponentInChildren<Text> ().text = "Tidal Surge";
			c_skillButtons [1].GetComponent<Button> ().onClick.AddListener(delegate{ActivateSkill ("TidalSurge");});
			Debug.Log ("Blue Mage set skill 2 to Tidal Surge");
			break;
		case "RedMage":
			c_skillButtons [0].GetComponentInChildren<Text> ().text = "Flare";
			c_skillButtons [0].GetComponent<Button> ().onClick.AddListener(delegate{ActivateSkill ("Flare");});
			Debug.Log ("Red Mage set skill 1 to Flare");
			c_skillButtons [1].GetComponentInChildren<Text> ().text = "Warmth";
			c_skillButtons [1].GetComponent<Button> ().onClick.AddListener(delegate{ActivateSkill ("Warmth");});
			Debug.Log ("Red Mage set skill 2 to Warmth");
			break;
		case "Tank":
			c_skillButtons [0].GetComponentInChildren<Text> ().text = "Stalwart";
			c_skillButtons [0].GetComponent<Button> ().onClick.AddListener(delegate{ActivateSkill ("Stalwart");});
			Debug.Log ("Tank set skill 1 to Stalwart");
			c_skillButtons [1].GetComponentInChildren<Text> ().text = "Crunch";
			c_skillButtons [1].GetComponent<Button> ().onClick.AddListener(delegate{ActivateSkill ("Crunch");});
			Debug.Log ("Tank set skill 2 to Crunch");
			break;
		case "Thief":
			c_skillButtons [0].GetComponentInChildren<Text> ().text = "Pounce";
			c_skillButtons [0].GetComponent<Button> ().onClick.AddListener(delegate{ActivateSkill ("Pounce");});
			Debug.Log ("Thief set skill 1 to Pounce");
			c_skillButtons [1].GetComponentInChildren<Text> ().text = "Swift Strike";
			c_skillButtons [1].GetComponent<Button> ().onClick.AddListener(delegate{ActivateSkill ("SwiftStrike");});
			Debug.Log ("Thief set skill 2 to Swift Strike");
			break;
		case "Warrior":
			c_skillButtons [0].GetComponentInChildren<Text> ().text = "Arm-Breaker";
			c_skillButtons [0].GetComponent<Button> ().onClick.AddListener(delegate{ActivateSkill ("ArmBreaker");});
			Debug.Log ("Thief set skill 1 to Arm-Breaker");
			c_skillButtons [1].GetComponentInChildren<Text> ().text = "War Cry";
			c_skillButtons [1].GetComponent<Button> ().onClick.AddListener(delegate{ActivateSkill ("WarCry");});
			Debug.Log ("Thief set skill 2 to War Cry");
			break;
		}			
	}

	public void PointerEnterSkillButton(int l_skillButtonNumber){
		c_skillDescCont.SetActive(true);

		string l_skillToDisplay = "";
		if (l_skillButtonNumber == 1)
			l_skillToDisplay = c_skillButtons [0].GetComponentInChildren<Text> ().text;
		else
			l_skillToDisplay = c_skillButtons [1].GetComponentInChildren<Text> ().text;

		AbstractSkill l_temp = new BasicAttack();

		switch(l_skillToDisplay){
		case "Warmth":
			l_temp = new Warmth ();
			break;
		case "Flare":
			l_temp = new FlareSkill ();
			break;
		case "Soothing River":
			l_temp = new SoothingRiver ();
			break;
		case "Tidal Surge":
			l_temp = new TidalSurge ();
			break;
		case "Arm-Breaker":
			l_temp = new ArmBreaker ();
			break;
		case "War Cry":
			l_temp = new WarCry ();
			break;
		case "Pounce":
			l_temp = new Pounce ();
			break;
		case "Swift Strike":
			l_temp = new SwiftStrike ();
			break;
		case "Stalwart":
			l_temp = new Stalwart ();
			break;
		case "Crunch":
			l_temp = new Crunch ();
			break;
		}
		c_skillTextComps [0].text = l_temp.getSkillName ();
		c_skillTextComps [1].text = "Power: " + l_temp.getSkillPower ();
		c_skillTextComps [2].text = "Range: " + l_temp.getPlayerSkillRange ();
		c_skillTextComps [3].text = "AOE: " + l_temp.getPlayerAOERange ();
		c_skillTextComps [4].text = l_temp.getSkillDescription ();
	}

	public void PointerExitSkillButton(){
		c_skillDescCont.SetActive(false);
	}

	private void ActivateSkill(string l_skillName){
		c_playerAttackScript.SetSkill(l_skillName);
		c_skillContainer.SetActive (false);
		c_playerAttackScript.MyAttack ();
	}

	public void DefendButtonClicked(){
		c_playerAttackScript.MyDefend ();
		c_skillContainer.SetActive (false);
	}

	public void MoveButtonClicked(){
		c_playerAttackScript.SetupMove ();
		c_skillContainer.SetActive (false);
	}

	public void CancelMoveClicked(){
		c_playerAttackScript.ResetMovement ();
		c_skillContainer.SetActive (false);
	}

	public void UpdateUIMoved(bool moved){//true means UI will display cancel button, false displays move
		if (moved) {
			c_movementButton.SetActive (false);
			c_resetMoveButton.SetActive (true);
		} else {
			c_movementButton.SetActive (true);
			c_resetMoveButton.SetActive (false);
		}
	}

	public void EndTurnButtonClicked(){
		c_playerAttackScript.EndTurn ();
		c_skillContainer.SetActive (false);
	}

	public void UpdateBattleDialogue(string l_battleText){
		c_battleDialogue.text = l_battleText;
	}

	public void CreateFloatingText(string l_textToDisplay, Color l_colourToDisplay, GameObject l_sender){
		c_floatingTextTest.SetText (l_textToDisplay, l_colourToDisplay, l_sender);
	}

	public void UpdateCamera(GameObject l_charToFloat){
		//c_main.transform.SetParent (l_charToFloat.transform);
		StartCoroutine (TransitionCamera (l_charToFloat.transform.position));
		c_main.transform.rotation = Quaternion.Euler(c_cameraRotation);
	}

	public void GameOver(string whoWon){
		UpdateBattleDialogue (whoWon);
		Invoke ("ReturnToMainMenu", 5f);
	}

	private void ReturnToMainMenu(){
		SceneManager.LoadScene (0);
	}

	private IEnumerator TransitionCamera(Vector3 l_charToFloat){

		float speed = ((l_charToFloat + c_cameraPositionOffset) - c_main.transform.position).normalized.magnitude;

		while (c_main.transform.position != l_charToFloat + c_cameraPositionOffset) 
		{
			Vector3 dir = ((l_charToFloat + c_cameraPositionOffset) - c_main.transform.position).normalized;
			c_main.transform.position += dir * (35f * Time.deltaTime) * speed * 2;

			if (Mathf.Abs (((l_charToFloat + c_cameraPositionOffset) - c_main.transform.position).magnitude) < 1)
				c_main.transform.position = l_charToFloat + c_cameraPositionOffset;
			yield return null;
		}
		StopCoroutine ("TransitionCamera");
	}
}

public class BattleDialogue
{
	public string c_attackerName;
	public int c_damage;

	public BattleDialogue(string l_attackerName, int l_damage){
		c_attackerName = l_attackerName;
		c_damage = l_damage;
	}
}

