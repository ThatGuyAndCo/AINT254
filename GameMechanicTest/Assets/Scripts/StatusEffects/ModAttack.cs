using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A status effect that changes the strength value used in the character's attack calculations.
/// Inherits from IStatusEffect.
/// </summary>
public class ModAttack : IStatusEffect {

	private int c_counter = 0;
	private int c_numOfTurns = 0;
	PlayerHealth c_afflicted;
	private int c_magnitude = 0;

	/// <summary>
	/// Initializes a new instance of the <see cref="ModAttack"/> class.
	/// </summary>
	/// <param name="l_magnitude">The magnitude of the buff.</param>
	/// <param name="l_numOfTurns">The number of turns the debuff lasts.</param>
	/// <param name="l_afflicted">The health script for the afflicted character.</param>
	public ModAttack(int l_magnitude, int l_numOfTurns, PlayerHealth l_afflicted){
		c_magnitude = l_magnitude;
		c_numOfTurns = l_numOfTurns;
		c_afflicted = l_afflicted;
		PreApply ();
	}

	public void Tick (){
		c_counter++;
		if (c_counter >= c_numOfTurns)
			RemoveEffect ();
	}
		
	public void PreApply(){
		bool l_noAttackBuff = true;
		bool l_noAttackDebuff = true;
		for (int i = 0; i < c_afflicted.c_statusEff.Count; i++) {
			if (c_afflicted.c_statusEff [i].GetType() == typeof(ModAttack)) {
				if (c_afflicted.c_statusEff [i].GetMagnitude () < 0 && c_magnitude < 0){
					if (c_afflicted.c_statusEff [i].GetMagnitude () > c_magnitude) {
						c_afflicted.c_statusEff [i].RemoveEffect ();
						ApplyEffect ();
						c_afflicted.c_UI.CreateFloatingText ("ATT DOWN", Color.red, c_afflicted.gameObject);
					}
					l_noAttackDebuff = false;
				} else if (c_afflicted.c_statusEff [i].GetMagnitude () > 0 && c_magnitude > 0){
					if (c_afflicted.c_statusEff [i].GetMagnitude () < c_magnitude) {
						c_afflicted.c_statusEff [i].RemoveEffect ();
						ApplyEffect ();
						c_afflicted.c_UI.CreateFloatingText ("ATT UP", Color.green, c_afflicted.gameObject);
					}
					l_noAttackBuff = false;
				} else if((c_afflicted.c_statusEff [i].GetMagnitude () < 0 && c_magnitude < 0 && c_magnitude >= c_afflicted.c_statusEff [i].GetMagnitude ()) || (c_afflicted.c_statusEff [i].GetMagnitude () > 0 && c_magnitude > 0 && c_magnitude <= c_afflicted.c_statusEff [i].GetMagnitude ())) {
					c_afflicted.c_statusEff[i].ResetCounter ();
					c_afflicted.c_UI.CreateFloatingText ("TURNS RESET", Color.magenta, c_afflicted.gameObject);
				}
			}
		}
		if (l_noAttackBuff && c_magnitude > 0) {
			ApplyEffect ();
			c_afflicted.c_UI.CreateFloatingText ("ATT UP", Color.green, c_afflicted.gameObject);
		} else if (l_noAttackDebuff && c_magnitude < 0) {
			ApplyEffect ();
			c_afflicted.c_UI.CreateFloatingText ("ATT DOWN", Color.red, c_afflicted.gameObject);
		}
	}

	public void ApplyEffect (){
		c_afflicted.c_playerStats.playerStrength += (int)((float)(c_afflicted.c_unmodifiedStats.playerStrength / 100.0f) * c_magnitude);
		c_afflicted.c_statusEff.Add (this);
	}

	public void RemoveEffect (){
		c_afflicted.c_playerStats.playerStrength -= (int)((float)(c_afflicted.c_unmodifiedStats.playerStrength / 100.0f) * c_magnitude);
		c_afflicted.c_statusEff.Remove (this);
	}

	public void ResetCounter (){
		if(c_numOfTurns / 2 < c_counter)
			c_counter = c_numOfTurns/2;
	}

	public int GetMagnitude(){
		return c_magnitude;
	}
}
