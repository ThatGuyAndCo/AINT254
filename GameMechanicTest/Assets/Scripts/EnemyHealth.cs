using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

	[SerializeField]
	private int c_maxHealth;

	[SerializeField] 
	private int c_currentHealth;

	// Use this for initialization
	void Start () {
		c_currentHealth = c_maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		if (c_currentHealth <= 0)
			Destroy (gameObject);
	}

	public void TakeDamage(int l_damage){
		c_currentHealth -= l_damage;
	}

	void OnDestroy(){
		gameObject.GetComponent<EnemyAttack> ().DeathDelay ();
	}
}
