using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class st_playerStats
{
	public int c_playerMaxHealth;
	public int playerDefence;
	public int playerStrength;
	public int playerSpeed;
	public int playerMoveRange;
	public int playerAttackRange;
}

public class PlayerHealth : MonoBehaviour 
{

	[SerializeField]
	private UIControl c_UI;

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

			if (GridTest.s_enemyCharacters.Count == 0) {
				c_UI.GameOver ("BlueTeamWin");
			} else if (GridTest.s_playerCharacters.Count == 0) {
				c_UI.GameOver ("RedTeamWin");
			}
		}
	}

	public void Defend()
	{
		c_playerDefend = true;
	}

	public void CancelDefend()
	{
		c_playerDefend = false;
	}

	public void TakeDamage(BattleDialogue l_takeDamage)
	{
		l_takeDamage.c_damage -= (int)Mathf.Min (l_takeDamage.c_damage * 0.3f, (float)(l_takeDamage.c_damage * (1 / c_playerStats.playerDefence)));
		if (c_playerDefend) {
			l_takeDamage.c_damage /= 2;
		}
		c_UI.UpdateBattleDialogue ("" + l_takeDamage.c_attackerName + " dealt " + l_takeDamage.c_damage + " damage to " + gameObject.name + ".");
		c_UI.CreateFloatingText ("" + l_takeDamage.c_damage, Color.red, gameObject);
		playerCurrentHealth -= l_takeDamage.c_damage;
		c_healthBar.value = ((float)playerCurrentHealth/(float)c_playerStats.c_playerMaxHealth) * 100;
	}

	void OnDestroy()
	{
		gameObject.GetComponent<PlayerAttack> ().DeathDelay ();
	}
}


