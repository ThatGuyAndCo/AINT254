using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
	Rigidbody2D rigidbody2D;
	public GameObject target;
	public float speed = 2.5f;
	Vector3[] path;
	Grid grid;
	int TargetIndex;
	bool findNewPath = false;
	float smoothing = 10f;
	int adjustmentAngle = -90;
    Vector3 testValue;
	public bool dead;

	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag ("Player");
		PathRequestManager.RequestPath (transform.position, target.transform.position, OnPathFound);
		rigidbody2D = GetComponent<Rigidbody2D> ();
	}

	void Update(){
		if (findNewPath) {
			PathRequestManager.RequestPath (transform.position, target.transform.position, OnPathFound);
			findNewPath = false;
		}
	}

	public void OnPathFound(Vector3[] newPath, bool pathSuccess){
        try
        {
            testValue = newPath[0];
        }
        catch
        {
            pathSuccess = false;
        }
		if (pathSuccess && this != null && !dead) {
			path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine ("FollowPath");
		}
        findNewPath = true;
    }

	IEnumerator FollowPath(){
        
        Vector3 currentWaypoint = path [0];

		while (true) {
			if (transform.position == currentWaypoint) {
				TargetIndex++;
				if (TargetIndex >= path.Length) {
					yield break;
				}
				currentWaypoint = path [TargetIndex];
			}

			if (dead) {
				break;
			}

			transform.position = Vector3.MoveTowards (transform.position, currentWaypoint, speed * Time.deltaTime);
			rigidbody2D.angularVelocity = 0.0f;
			rigidbody2D.velocity = Vector2.zero;
			Vector3 difference = currentWaypoint - transform.position;
			difference.Normalize();
			float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
			Quaternion newRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, rotZ + adjustmentAngle));
			transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * smoothing);
			yield return null;
		}
	}

	public void OnDrawGizmos(){
		if (path != null) {
			for (int i = 0; i < path.Length; i++){
				Gizmos.color = Color.black;
				Gizmos.DrawCube (path [i], Vector3.one * (0.2f - 0.1f));

				if (i == TargetIndex) {
					Gizmos.DrawLine (transform.position, path [i]);
				} else if (i != 0){
					Gizmos.DrawLine (path [i - 1], path [i]);
				}
			}
		}
	}

	public void die(){
		transform.position = Vector3.MoveTowards (transform.position, transform.position, 1f);
		dead = true;
		StopAllCoroutines ();
	}

	public void OnDestroy(){
		StopAllCoroutines ();
	}

	public void knockback(float knockbackSpeed){
		StopCoroutine ("FollowPath");
		findNewPath = false;
		Invoke ("resetAlg", 0.35f);
		rigidbody2D.velocity = -(transform.up * knockbackSpeed);
	}

	void resetAlg(){
		findNewPath = true;
	}
}
