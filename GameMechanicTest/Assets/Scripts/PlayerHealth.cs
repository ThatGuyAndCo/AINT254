using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

[Serializable]
public class st_playerStats
{
	public int c_playerMaxHealth;
	public int playerDefence;
	public int playerStrength;
	public int playerSpeed;
	public int playerMoveRange;
	public int playerAttackRange;
	public int c_power;
}

public class PlayerHealth : MonoBehaviour 
{

	public UIControl c_UI;

	[SerializeField]
	private int playerCurrentHealth;

	public st_playerStats c_playerStats;

	private bool c_playerDefend = false;

	[SerializeField]
	private Slider c_healthBar;

	// Use this for initialization
	void Start () 
	{
		c_UI = GameObject.FindGameObjectWithTag ("UICanvas").GetComponent<UIControl>();
		playerCurrentHealth = c_playerStats.c_playerMaxHealth;
		c_healthBar.value = ((float)playerCurrentHealth/(float)c_playerStats.c_playerMaxHealth) * 100;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (playerCurrentHealth <= 0) {
			Destroy (gameObject);
			if (gameObject.CompareTag ("EnemyTeam"))
				GridTest.s_enemyCharacters.Remove (gameObject);
			else
				GridTest.s_playerCharacters.Remove (gameObject);


		}
	}

	public void Defend()
	{
		c_UI.CreateFloatingText ("Defend", Color.red, gameObject);
		c_playerDefend = true;
	}

	public void CancelDefend()
	{
		c_playerDefend = false;
	}

	private int DamageCalculator(int l_baseDamage){
		int returnDamage = l_baseDamage;

		if (Random.Range (0, 100) < c_playerStats.playerSpeed) {
			returnDamage = 0;
			c_UI.CreateFloatingText ("Miss", Color.green, gameObject);
		}

		return returnDamage;
	}

	public void TakeDamage(BattleDialogue l_takeDamage)
	{
		Debug.Log ("Base = " + l_takeDamage.c_damage + ", 30% = " + l_takeDamage.c_damage * 0.3f + ", defence calc = " + DamageCalculator(l_takeDamage.c_damage));
		if (l_takeDamage.c_damage > -1) {
			l_takeDamage.c_damage = (int)Mathf.Max (l_takeDamage.c_damage * 0.3f, (float)(DamageCalculator(l_takeDamage.c_damage)));
			if (c_playerDefend) {
				l_takeDamage.c_damage /= 2;
			}
			c_UI.UpdateBattleDialogue ("" + l_takeDamage.c_attackerName + " dealt " + l_takeDamage.c_damage + " damage to " + gameObject.name + ".");
			c_UI.CreateFloatingText ("" + l_takeDamage.c_damage, Color.red, gameObject);
		} else {
			c_UI.UpdateBattleDialogue ("" + l_takeDamage.c_attackerName + " healed " + l_takeDamage.c_damage + " damage to " + gameObject.name + ".");
			c_UI.CreateFloatingText ("" + l_takeDamage.c_damage, Color.green, gameObject);
		}
		playerCurrentHealth -= l_takeDamage.c_damage;
		c_healthBar.value = ((float)playerCurrentHealth/(float)c_playerStats.c_playerMaxHealth) * 100;
	}

	public void TakeDamage(float l_damagePercent)
	{
		int l_takeDamage = (int)(c_playerStats.c_playerMaxHealth / l_damagePercent);
		Debug.Log ("Base = " + l_takeDamage + ", 30% = " + l_takeDamage * 0.3f + ", defence calc = " + DamageCalculator(l_takeDamage));
		l_takeDamage = (int)Mathf.Max (l_takeDamage * 0.3f, (float)(DamageCalculator(l_takeDamage)));
		if (c_playerDefend) {
			l_takeDamage /= 2;
		}
		c_UI.CreateFloatingText ("" + l_takeDamage, Color.red, gameObject);
		playerCurrentHealth -= l_takeDamage;
		c_healthBar.value = ((float)playerCurrentHealth/(float)c_playerStats.c_playerMaxHealth) * 100;
	}

	void OnDestroy()
	{
		gameObject.GetComponent<PlayerAttack> ().DeathDelay ();
		if (GridTest.s_enemyCharacters.Count == 0) {
			c_UI.GameOver ("Blue Team Win");
		} else if (GridTest.s_playerCharacters.Count == 0) {
			c_UI.GameOver ("Red Team Win");
		}
	}

	public int GetDefence(){
		return c_playerStats.playerDefence;
	}
}


