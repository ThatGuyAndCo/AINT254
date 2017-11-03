using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


	private FloatingDamageTest c_floatingTextTest;

	void Start(){
		c_battleDialogue.text = "";
		c_floatingTextTest = GetComponent<FloatingDamageTest> ();
	}

	public void UpdateActiveCharacter(GameObject l_activeCharacter){
		c_activeCharacter = l_activeCharacter;
		c_playerAttackScript = c_activeCharacter.GetComponent<PlayerAttack> ();
		c_characterName.text = "" + l_activeCharacter.name + "'s turn";
		UpdateCamera (l_activeCharacter);
	}

	public void DynamicHide(bool l_hide){ //true = display, false = hidden
		c_displayContainer.SetActive(l_hide);
	}

	public void AttackButtonClicked(){
		c_playerAttackScript.MyAttack ();
	}

	public void DefendButtonClicked(){
		c_playerAttackScript.MyDefend ();
	}

	public void MoveButtonClicked(){
		c_playerAttackScript.SetupMove ();
	}

	public void EndTurnButtonClicked(){
		c_playerAttackScript.EndTurn ();
	}

	public void UpdateBattleDialogue(string l_battleText){
		c_battleDialogue.text = l_battleText;
	}

	public void CreateFloatingText(string l_textToDisplay, Color l_colourToDisplay, GameObject l_sender){
		c_floatingTextTest.SetText (l_textToDisplay, l_colourToDisplay, l_sender);
	}

	public void UpdateCamera(GameObject l_charToFloat){
		c_main.transform.SetParent (l_charToFloat.transform);
		c_main.transform.position = l_charToFloat.transform.position + c_cameraPositionOffset;
		c_main.transform.rotation = Quaternion.Euler(c_cameraRotation);
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

