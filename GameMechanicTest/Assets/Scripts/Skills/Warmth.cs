using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warmth : AbstractSkill {

	protected int c_baseDamage = -100;
	public int c_skillRange = 6;
	protected int c_AOERange = 0;
	protected float c_turnDelayModifier = 1.2f;

	public override float UseSkill (Vector3 l_target, PlayerHealth l_myStats, string l_targetTeamTag){
		List<GameObject> l_targets = TargetsInRange(l_target, c_AOERange, l_targetTeamTag);
		for (int t = 0; t < l_targets.Count; t++) {
			PlayerHealth l_currentTarget = l_targets [t].GetComponent<PlayerHealth> ();
			ApplyEffectToTarget (l_currentTarget, c_baseDamage, l_myStats);
		}
		return c_turnDelayModifier;
	}

	public override int getPlayerSkillRange(){
		return c_skillRange;
	}
}
