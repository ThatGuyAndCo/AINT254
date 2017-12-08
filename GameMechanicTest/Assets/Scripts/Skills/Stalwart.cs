using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalwart : AbstractSkill {

	protected int c_baseDamage = 65;
	public int c_skillRange = 0;
	protected int c_AOERange = 3;
	protected float c_turnDelayModifier = 1.3f;

	public override float UseSkill (Vector3 l_target, PlayerHealth l_myStats, string l_targetTeamTag){
		List<GameObject> l_targets = TargetsInRange(l_target, c_AOERange, l_targetTeamTag);
		for (int t = 0; t < l_targets.Count; t++) {
			PlayerHealth l_currentTarget = l_targets [t].GetComponent<PlayerHealth> ();
			ApplyEffectToTarget (l_currentTarget, c_baseDamage, l_myStats);
		}
		l_myStats.TakeDamage (-10);
		return c_turnDelayModifier;
	}

	protected List<GameObject> TargetsInRange (Vector3 l_targetPos, int l_AOERange, string l_targetTeamTag){
		return GridTest.CheckRange (l_targetPos, l_AOERange, l_targetTeamTag);
	}

	protected virtual void ApplyEffectToTarget(PlayerHealth l_enemy, int l_damageToDeal, PlayerHealth l_instigator){
		IStatusEffect l_temp = new ModDefence (20, 3, l_enemy);
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
		return "Shields the team-mates in range, raising their defence.";
	}

	public override string getSkillName (){
		return "Stalwart";
	}
}
