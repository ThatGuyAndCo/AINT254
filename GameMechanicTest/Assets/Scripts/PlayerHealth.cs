using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

	[SerializeField]
	private UIControl c_UI;

	[SerializeField]
	private int c_playerMaxHealth;

	[SerializeField] 
	private int c_playerCurrentHealth;
	private bool c_playerDefend = false;

	[SerializeField]
	private Slider c_healthBar;

	private PlayerMove c_playerMove;

	// Use this for initialization
	void Start () {
		c_UI = GameObject.FindGameObjectWithTag ("UICanvas").GetComponent<UIControl>();
		c_playerCurrentHealth = c_playerMaxHealth;
		c_healthBar.value = ((float)c_playerCurrentHealth/(float)c_playerMaxHealth) * 100;
		c_playerMove = GetComponent<PlayerMove> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (c_playerCurrentHealth <= 0)
			Destroy (gameObject);
	}

	public void Defend(){
		c_playerDefend = true;
	}

	public void Move (){
		Debug.Log ("Calling Initiate Move");
		c_playerMove.InitiateMove (transform.position, new Vector3 (75,0,55), true);
	}

	public void CancelDefend(){
		c_playerDefend = false;
	}

	public void TakeDamage(BattleDialogue l_takeDamage){
		if (c_playerDefend) {
			l_takeDamage.c_damage /= 2;
		}
		c_UI.UpdateBattleDialogue ("" + l_takeDamage.c_attackerName + " dealt " + l_takeDamage.c_damage + " damage to " + gameObject.name + ".");
		c_UI.CreateFloatingText ("" + l_takeDamage.c_damage, Color.red, gameObject);
		c_playerCurrentHealth -= l_takeDamage.c_damage;
		c_healthBar.value = ((float)c_playerCurrentHealth/(float)c_playerMaxHealth) * 100;
	}

	void OnDestroy(){
		gameObject.GetComponent<PlayerAttack> ().DeathDelay ();
	}
}
