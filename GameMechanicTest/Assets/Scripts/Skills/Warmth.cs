﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warmth : AbstractSkill {

	protected int c_baseDamage = -25;
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

	protected override void ApplyEffectToTarget(PlayerHealth l_enemy, int l_magnitude, PlayerHealth l_instigator){
		ContinuedEffect l_temp = new HPRegen(l_magnitude, 5, l_enemy);
	}

	public override int getPlayerSkillRange(){
		return c_skillRange;
	}

	public override int getSkillPower (){
		return -c_baseDamage;
	}

	public override string getSkillDescription (){
		return "A warm light gently heals the team-mate for a percentage of thier health per turn, for 5 turns (non-stackable).";
	}
	public override int getPlayerAOERange(){
		return c_AOERange;
	}

	public override string getSkillName (){
		return "Warmth";
	}
}
