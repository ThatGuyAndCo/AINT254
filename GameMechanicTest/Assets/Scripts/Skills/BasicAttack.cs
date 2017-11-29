using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : AbstractSkill{

	protected int c_baseDamage = 50;
	public int c_skillRange = 1;
	protected int c_AOERange = 0;
	protected float c_turnDelayModifier = 1.0f;

	public override float UseSkill (Vector3 l_target, PlayerHealth l_myStats, string l_targetTeamTag){
		List<GameObject> l_targets = TargetsInRange(l_target, c_AOERange, l_targetTeamTag);
		for (int t = 0; t < l_targets.Count; t++) {
			PlayerHealth l_currentTarget = l_targets [t].GetComponent<PlayerHealth> ();
			int l_damageToDeal = CalculateDamage (l_currentTarget, l_myStats, c_baseDamage);
			ApplyEffectToTarget (l_currentTarget, l_damageToDeal, l_myStats);
		}
		return c_turnDelayModifier;
	}

	public override int getPlayerSkillRange(){
		return c_skillRange;
	}
}
