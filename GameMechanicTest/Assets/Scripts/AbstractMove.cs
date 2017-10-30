using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMove : MonoBehaviour {

	protected abstract Vector3 CalculateMove (Vector3 l_startPos, Vector3 l_endPos, List<Vector3> l_ignoreNode);

	protected abstract Vector3[] CalculatePath(Vector3 l_startPos, Vector3 l_endPos);

	protected abstract bool IsThereObstruction (Vector3 l_node);

	protected abstract IEnumerator MoveToNextNodeCo (Vector3[] l_pathToFollow);

	//protected abstract bool IsTurnOver(Vector3 l_startPos, Vector3 l_endPos);

	public abstract string InitiateMove(Vector3 l_startPos, Vector3 l_endPos);
}
