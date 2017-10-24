using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InterfaceMove : MonoBehaviour {

	Vector3 c_startPos;
	Vector3 c_endPos;

	protected abstract void CalculateMove (Vector3 l_startPos, Vector3 l_endPos);

	protected abstract void CalculatePath(Vector3 l_startPos, Vector3 l_endPos);

	protected abstract bool IsThereObstruction (Vector3[] l_path);

	protected abstract void MoveToNextNode(Vector3 l_currentNode, Vector3 l_nextNode);

	protected abstract bool IsTurnOver(Vector3 l_startPos, Vector3 l_endPos);

	public abstract string InitiateMove(Vector3 l_startPos, Vector3 l_endPos, bool l_playerAttacked);
}
