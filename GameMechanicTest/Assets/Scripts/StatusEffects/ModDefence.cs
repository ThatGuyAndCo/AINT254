using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A status effect that changes the defence value used in the enemy's attack calculations.
/// Inherits from IStatusEffect.
/// </summary>
public class ModDefence : IStatusEffect {

	private int c_counter = 0;
	private int c_numOfTurns = 0;
	PlayerHealth c_afflicted;
	private int c_magnitude = 0;

	/// <summary>
	/// Initializes a new instance of the <see cref="ModDefence"/> class.
	/// </summary>
	/// <param name="l_magnitude">The magnitude of the buff.</param>
	/// <param name="l_numOfTurns">The number of turns the debuff lasts.</param>
	/// <param name="l_afflicted">The health script for the afflicted character.</param>
	public ModDefence(int l_magnitude, int l_numOfTurns, PlayerHealth l_afflicted){
		c_magnitude = l_magnitude;
		c_numOfTurns = l_numOfTurns;
		c_afflicted = l_afflicted;
		PreApply ();
	}

	public void Tick (){
		c_counter++;
		if (c_counter > c_numOfTurns)
			RemoveEffect ();
	}

	public void PreApply(){
		bool l_noDefBuff = true;
		bool l_noDefDebuff = true;
		for (int i = 0; i < c_afflicted.c_statusEff.Count; i++) {
			if (c_afflicted.c_statusEff [i].GetType() == typeof(ModDefence)) {
				if (c_afflicted.c_statusEff [i].GetMagnitude () < 0 && c_magnitude < 0){
					if (c_afflicted.c_statusEff [i].GetMagnitude () > c_magnitude) {
						c_afflicted.c_statusEff [i].RemoveEffect ();
						ApplyEffect ();
						c_afflicted.c_UI.CreateFloatingText ("DEF DOWN", Color.red, c_afflicted.gameObject);
					}
					l_noDefDebuff = false;
				} else if (c_afflicted.c_statusEff [i].GetMagnitude () > 0 && c_magnitude > 0){
					c_afflicted.c_statusEff [i].RemoveEffect ();
					ApplyEffect ();
					l_noDefBuff = false;
					c_afflicted.c_UI.CreateFloatingText ("DEF UP", Color.green, c_afflicted.gameObject);
				} else if((c_afflicted.c_statusEff [i].GetMagnitude () < 0 && c_magnitude < 0 && c_magnitude >= c_afflicted.c_statusEff [i].GetMagnitude ()) || (c_afflicted.c_statusEff [i].GetMagnitude () > 0 && c_magnitude > 0 && c_magnitude <= c_afflicted.c_statusEff [i].GetMagnitude ())) {
					c_afflicted.c_statusEff[i].ResetCounter ();
					c_afflicted.c_UI.CreateFloatingText ("TURNS RESET", Color.magenta, c_afflicted.gameObject);
				}
			}
		}
		if (l_noDefBuff && c_magnitude > 0) {
			ApplyEffect ();
			c_afflicted.c_UI.CreateFloatingText ("DEF UP", Color.green, c_afflicted.gameObject);
		} else if (l_noDefDebuff && c_magnitude < 0) {
			ApplyEffect ();
			c_afflicted.c_UI.CreateFloatingText ("DEF DOWN", Color.red, c_afflicted.gameObject);
		}
	}

	public void ApplyEffect (){
		c_afflicted.c_playerStats.playerDefence += (int)((float)(c_afflicted.c_unmodifiedStats.playerDefence / 100.0f) * c_magnitude);
		c_afflicted.c_statusEff.Add (this);
	}

	public void RemoveEffect (){
		c_afflicted.c_playerStats.playerDefence -= (int)((float)(c_afflicted.c_unmodifiedStats.playerDefence / 100.0f) * c_magnitude);
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
