using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareSkill : AbstractSkill {

	protected int c_baseDamage = 36;
	public int c_skillRange = 5;
	protected int c_AOERange = 3;
	protected float c_turnDelayModifier = 1.3f;

	public override float UseSkill (Vector3 l_target, PlayerHealth l_myStats, string l_targetTeamTag){
		List<GameObject> l_targets = TargetsInRange(l_target, c_AOERange, l_targetTeamTag);
		for (int t = 0; t < l_targets.Count; t++) {
			PlayerHealth l_currentTarget = l_targets [t].GetComponent<PlayerHealth> ();
			int l_damageToDeal = CalculateDamage (l_currentTarget, l_myStats, c_baseDamage);
			ApplyEffectToTarget (l_currentTarget, l_damageToDeal, l_myStats);
		}
		l_myStats.TakeDamage (25);
		return c_turnDelayModifier;
	}

	public override int getPlayerSkillRange(){
		return c_skillRange;
	}

	public override int getPlayerAOERange(){
		return c_AOERange;
	}

	public override int getSkillPower (){
		return c_baseDamage;
	}

	public override string getSkillDescription (){
		return "A blast of fire that damages all enemies in range.";
	}

	public override string getSkillName (){
		return "Flare";
	}
}
