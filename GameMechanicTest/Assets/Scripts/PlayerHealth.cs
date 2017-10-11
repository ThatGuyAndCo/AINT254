using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	[SerializeField]
	private int c_playerMaxHealth;

	[SerializeField] 
	private int c_playerCurrentHealth;
	private bool c_playerDefend = false;

	// Use this for initialization
	void Start () {
		c_playerCurrentHealth = c_playerMaxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		if (c_playerCurrentHealth <= 0)
			Destroy (gameObject);
	}

	public void Defend(){
		c_playerDefend = true;
	}

	public void CancelDefend(){
		c_playerDefend = false;
	}

	public void TakeDamage(int l_damage){
		if (c_playerDefend) {
			l_damage /= 2;
		}
		c_playerCurrentHealth -= l_damage;
	}

	void OnDestroy(){
		gameObject.GetComponent<PlayerAttack> ().DeathDelay ();
	}
}
