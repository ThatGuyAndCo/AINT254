using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddGridToRange : MonoBehaviour {

	public GameObject GetEnemyOnTile(string l_tagToSearch){
		GameObject l_enemyOnTile = null;
		RaycastHit l_pHit = new RaycastHit();
        RaycastHit l_eHit = new RaycastHit();
        if (l_tagToSearch == "EnemyTeam" || l_tagToSearch == "") {
			Physics.Raycast (transform.position + new Vector3(0, 50, 0), -Vector3.up * 50, out l_pHit, 100f, 1 << 9);
		}
        if (l_tagToSearch == "PlayerTeam" || l_tagToSearch == "") {
			Physics.Raycast (transform.position + new Vector3(0, 50, 0), -Vector3.up * 50, out l_eHit, 100f, 1 << 8);
		}
		if (l_pHit.collider != null) {
			l_enemyOnTile = l_pHit.collider.gameObject;
			Debug.Log (l_enemyOnTile.name);
		}
        else if (l_eHit.collider != null)
        {
            l_enemyOnTile = l_eHit.collider.gameObject;
            Debug.Log(l_enemyOnTile.name);
        }
        return l_enemyOnTile;
	}
}
