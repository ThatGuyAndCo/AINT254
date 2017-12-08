using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSkill{

	/// <summary>
	/// Start the process of using the skill.
	/// </summary>
	/// <param name="l_target">The position the effect will be targeted around.</param>
	/// <param name="l_myStats">Link to the player stats (for level, attack, buffs/debuffs).</param>
	public abstract float UseSkill (Vector3 l_target, PlayerHealth l_myStats, string l_targetTeamTag);

	/// <summary>
	/// Targetses the in range.
	/// </summary>
	/// <returns>The in range.</returns>
	protected List<GameObject> TargetsInRange (Vector3 l_targetPos, int l_AOERange, string l_targetTeamTag){
		return GridTest.CheckRange (l_targetPos, l_AOERange, l_targetTeamTag);
	}

	/// <summary>
	/// Uses the skill on the target.
	/// </summary>
	protected virtual void ApplyEffectToTarget(PlayerHealth l_enemy, int l_damageToDeal, PlayerHealth l_instigator){
		BattleDialogue l_sendDamage = new BattleDialogue (l_instigator.name, l_damageToDeal);
		l_enemy.TakeDamage (l_sendDamage);
	}

	/// <summary>
	/// Calculates the damage for the specific target.
	/// </summary>
	/// <param name="l_enemy">The enemy being targeted (for defence and creating Crit text)</param>
	/// <returns>The final damage calculated</returns>
	protected virtual int CalculateDamage(PlayerHealth l_enemy, PlayerHealth l_instigator, int l_baseDamage){
		int returnDamage = 0;
		returnDamage = (int)(((((((l_instigator.c_playerStats.c_power * 2.0f) / 5.0f) + 2.0f) * l_baseDamage * ((float)l_instigator.c_playerStats.playerStrength / l_enemy.GetDefence())) / 50.0f) + 2.0f));
		if (Random.Range (0, 85) < l_instigator.c_playerStats.playerSpeed) {
			returnDamage = (int)( returnDamage * Mathf.Max(2.5f, (l_instigator.c_playerStats.playerSpeed / 3.0f)));
			l_instigator.c_UI.CreateFloatingText ("Crit!", Color.magenta, l_enemy.gameObject);
		}
		return returnDamage;
	}

	public abstract int getPlayerSkillRange ();

	public abstract int getPlayerAOERange ();

	public abstract int getSkillPower ();

	public abstract string getSkillDescription ();

	public abstract string getSkillName ();
}
