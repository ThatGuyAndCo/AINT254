using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour {

	[SerializeField]
	private UIControl c_UI;

	[SerializeField]
	private int c_maxHealth;

	[SerializeField] 
	private int c_currentHealth;

	[SerializeField]
	private Slider c_healthBar;

	// Use this for initialization
	void Start () {
		c_UI = GameObject.FindGameObjectWithTag ("UICanvas").GetComponent<UIControl>();
		c_currentHealth = c_maxHealth;
		c_healthBar.value = ((float)c_currentHealth / (float)c_maxHealth) * 100;
	}
	
	// Update is called once per frame
	void Update () {
		if (c_currentHealth <= 0)
			Destroy (gameObject);
	}

	public void TakeDamage(BattleDialogue l_takeDamage){
		c_UI.UpdateBattleDialogue ("" + l_takeDamage.c_attackerName + " dealt " + l_takeDamage.c_damage + " damage to " + gameObject.name + ".");
		c_UI.CreateFloatingText ("" + l_takeDamage.c_damage, Color.red, gameObject);
		c_currentHealth -= l_takeDamage.c_damage;
		c_healthBar.value = ((float)c_currentHealth / (float)c_maxHealth) * 100;
	}

	void OnDestroy(){
		gameObject.GetComponent<EnemyAttack> ().DeathDelay ();
	}
}
