using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddGridToRange : MonoBehaviour {

	public GameObject GetEnemyOnTile(string l_tagToSearch){
		GameObject l_enemyOnTile = null;
		RaycastHit l_hit = new RaycastHit();
		if (l_tagToSearch == "EnemyTeam") {
			Physics.Raycast (transform.position + new Vector3(0, 50, 0), -Vector3.up * 50, out l_hit, 100f, 1 << 9);
		} else if (l_tagToSearch == "PlayerTeam") {
			Physics.Raycast (transform.position + new Vector3(0, 50, 0), -Vector3.up * 50, out l_hit, 100f, 1 << 8);
		}
		if (l_hit.collider != null) {
			l_enemyOnTile = l_hit.collider.gameObject;
			Debug.Log (l_enemyOnTile.name);
		}
		return l_enemyOnTile;
	}
}
