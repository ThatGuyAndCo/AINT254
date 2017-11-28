using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractSkill{

	protected PlayerHealth c_instigator;
	protected string c_targetTeamTag;
	protected int c_baseDamage;
	protected int c_skillRange;
	protected int c_AOERange;
	protected float c_turnDelayModifier;
	protected Vector3 c_targetPos;

	public AbstractSkill(){

	}

	/// <summary>
	/// Start the process of using the skill.
	/// </summary>
	/// <param name="l_target">The position the effect will be targeted around.</param>
	/// <param name="l_myStats">Link to the player stats (for level, attack, buffs/debuffs).</param>
	public virtual float UseSkill (Vector3 l_target, PlayerHealth l_myStats, string l_targetTeamTag){
		c_instigator = l_myStats;
		c_targetTeamTag = l_targetTeamTag;
		c_targetPos = l_target;
		List<GameObject> l_targets = TargetsInRange();
		for (int t = 0; t < l_targets.Count; t++) {
			PlayerHealth l_currentTarget = l_targets [t].GetComponent<PlayerHealth> ();
			int l_damageToDeal = CalculateDamage (l_currentTarget);
			ApplyEffectToTarget (l_currentTarget, l_damageToDeal);
		}
		return c_turnDelayModifier;
	}

	/// <summary>
	/// Targetses the in range.
	/// </summary>
	/// <returns>The in range.</returns>
	protected List<GameObject> TargetsInRange (){
		return GridTest.CheckRange (c_targetPos, c_AOERange, c_targetTeamTag);
	}

	/// <summary>
	/// Uses the skill on the target.
	/// </summary>
	protected virtual void ApplyEffectToTarget(PlayerHealth l_enemy, int l_damageToDeal){
		BattleDialogue l_sendDamage = new BattleDialogue (c_instigator.name, l_damageToDeal);
		l_enemy.TakeDamage (l_sendDamage);
	}

	/// <summary>
	/// Calculates the damage for the specific target.
	/// </summary>
	/// <param name="l_enemy">The enemy being targeted (for defence and creating Crit text)</param>
	/// <returns>The final damage calculated</returns>
	protected int CalculateDamage(PlayerHealth l_enemy){
		int returnDamage = 0;
		returnDamage = (int)(((((((c_instigator.c_playerStats.c_power * 2.0f) / 5.0f) + 2.0f) * c_baseDamage * ((float)c_instigator.c_playerStats.playerStrength / l_enemy.GetDefence())) / 50.0f) + 2.0f));
		if (Random.Range (0, 85) < c_instigator.c_playerStats.playerSpeed) {
			returnDamage = (int)( returnDamage * Mathf.Max(2.5f, (c_instigator.c_playerStats.playerSpeed / 3.0f)));
			c_instigator.c_UI.CreateFloatingText ("Crit!", Color.red, l_enemy.gameObject);
		}
		return returnDamage;
	}


}
