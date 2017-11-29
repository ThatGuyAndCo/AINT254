using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pounce : AbstractSkill {

	protected int c_baseDamage = 45;
	public int c_skillRange = 1;
	protected int c_AOERange = 0;
	protected float c_turnDelayModifier = 1.3f;

	public override float UseSkill (Vector3 l_target, PlayerHealth l_myStats, string l_targetTeamTag){
		List<GameObject> l_targets = TargetsInRange(l_target, c_AOERange, l_targetTeamTag);
		for (int t = 0; t < l_targets.Count; t++) {
			PlayerHealth l_currentTarget = l_targets [t].GetComponent<PlayerHealth> ();
			int l_damageToDeal = CalculateDamage (l_currentTarget, l_myStats, c_baseDamage);
			ApplyEffectToTarget (l_currentTarget, l_damageToDeal, l_myStats);
		}
		l_myStats.TakeDamage (15);
		return c_turnDelayModifier;
	}

	protected override int CalculateDamage(PlayerHealth l_enemy, PlayerHealth l_instigator, int l_baseDamage){
		int returnDamage = 0;
		returnDamage = (int)(((((((l_instigator.c_playerStats.c_power * 2.0f) / 5.0f) + 2.0f) * l_baseDamage * ((float)l_instigator.c_playerStats.playerStrength / l_enemy.GetDefence())) / 50.0f) + 2.0f));
		if (Random.Range (0, 50) < l_instigator.c_playerStats.playerSpeed) {
			returnDamage = (int)( returnDamage * Mathf.Max(2.5f, (l_instigator.c_playerStats.playerSpeed / 3.0f)));
			l_instigator.c_UI.CreateFloatingText ("Crit!", Color.magenta, l_enemy.gameObject);
		}
		return returnDamage;
	}


	public override int getPlayerSkillRange(){
		return c_skillRange;
	}
}
