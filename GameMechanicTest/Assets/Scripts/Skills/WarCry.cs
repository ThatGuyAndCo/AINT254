using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarCry : AbstractSkill {

	protected int c_baseDamage = 0;
	public int c_skillRange = 0;
	protected int c_AOERange = 3;
	protected float c_turnDelayModifier = 1.3f;

	public override float UseSkill (Vector3 l_target, PlayerHealth l_myStats, string l_targetTeamTag){
		l_targetTeamTag = l_myStats.GetComponent<PlayerAttack> ().c_enemyDamageTag;
		List<GameObject> l_targets = TargetsInRange(l_target, c_AOERange, l_targetTeamTag);
		l_targets.AddRange (TargetsInRange (l_target, c_AOERange, l_myStats.gameObject.tag));
		for (int t = 0; t < l_targets.Count; t++) {
			PlayerHealth l_currentTarget = l_targets [t].GetComponent<PlayerHealth> ();
			if (l_targets [t].CompareTag (l_myStats.gameObject.tag)) {
				ApplyEffectToAllies (l_currentTarget, c_baseDamage, l_myStats);
			} else {
				ApplyEffectToTarget (l_currentTarget, -c_baseDamage, l_myStats);
			}
		}
		l_myStats.TakeDamage (-5);
		IStatusEffect l_temp = new ModDefence (-50, 3, l_myStats);
		return c_turnDelayModifier;
	}

	protected override void ApplyEffectToTarget(PlayerHealth l_enemy, int l_damageToDeal, PlayerHealth l_instigator){
		IStatusEffect l_temp = new ModDefence (-10, 3, l_enemy);
		l_temp = new ModAttack (-10, 3, l_enemy);
	}

	private void ApplyEffectToAllies(PlayerHealth l_enemy, int l_damageToDeal, PlayerHealth l_instigator){
		IStatusEffect l_temp = new ModAttack (10, 3, l_enemy);
	}

	public override int getPlayerSkillRange(){
		return c_skillRange;
	}
}
