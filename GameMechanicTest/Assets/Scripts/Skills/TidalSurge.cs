using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TidalSurge : AbstractSkill {

	protected int c_baseDamage = 20;
	public int c_skillRange = 0;
	protected int c_AOERange = 6;
	protected float c_turnDelayModifier = 1.2f;

	public override float UseSkill (Vector3 l_target, PlayerHealth l_myStats, string l_targetTeamTag){
		l_targetTeamTag = l_myStats.GetComponent<PlayerAttack> ().c_enemyDamageTag;
		List<GameObject> l_targets = TargetsInRange(l_target, c_AOERange, l_targetTeamTag);
		for (int t = 0; t < l_targets.Count; t++) {
			PlayerHealth l_currentTarget = l_targets [t].GetComponent<PlayerHealth> ();
			int l_damageToDeal = CalculateDamage (l_currentTarget, l_myStats, c_baseDamage);
			ApplyEffectToTarget (l_currentTarget, l_damageToDeal, l_myStats);
		}
		l_myStats.TakeDamage (15);
		return c_turnDelayModifier;
	}

	public override int getPlayerSkillRange(){
		return c_skillRange;
	}
}
