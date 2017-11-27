using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMoveAStar : AbstractMove {

	private Transform c_myTrans;
	/// <summary>
	/// A pointer for the game object's transform is set here to make accessing it later in the Coroutine faster.
	/// </summary>
	void Start(){
		c_myTrans = transform;
	}

	/// <summary>
	/// Calculates the path.
	/// </summary>
	/// <returns>The path.</returns>
	/// <param name="l_startPos">L start position.</param>
	/// <param name="l_endPos">L end position.</param>
	private Vector3[] CalculatePath(Vector3 l_startPos, Vector3 l_endPos){
		List<Node> l_openList = new List<Node>();
		List<Node> l_closedList = new List<Node>();
		int[] l_tempGrid = GridTest.GetArrayPosFromVector (l_endPos);
		Node l_endNode = GridTest.s_gridPosArray [l_tempGrid[0], l_tempGrid[1]];
		Vector3[] l_returnArray = new Vector3[0];
		bool l_foundPath = false;
		l_tempGrid = GridTest.GetArrayPosFromVector (l_startPos);
		Node l_startNode = GridTest.s_gridPosArray [l_tempGrid[0], l_tempGrid[1]];
		l_openList.Add (l_startNode);

		while (l_openList.Count > 0) {
			Node l_nextNode = new Node(new Vector3(0,0,0));
			l_nextNode.c_gCost = 5000;
			l_nextNode.c_fCost = 5000;

			for (int n = 0; n < l_openList.Count; n++) {
				if (l_openList[n].c_fCost < l_nextNode.c_fCost)
					l_nextNode = l_openList[n];
			}

			l_openList.Remove (l_nextNode);
			l_closedList.Add (l_nextNode);

			if (l_nextNode.c_nodePosition == l_endPos) {
				l_foundPath = true;
				l_endNode = l_nextNode;
				Debug.Log ("Found path, breaking");
				Debug.Log ("Node parent = " + l_endNode.c_parentNode.c_nodePosition);
				break;
			}

			List<Node> l_neighbourNodes = FindNeighbours (l_openList, l_closedList, l_nextNode);

			for (int n = 0; n < l_neighbourNodes.Count; n++) {
				if (IsThereObstruction (l_neighbourNodes[n].c_nodePosition) || ListContains(l_closedList, l_neighbourNodes[n])) {
					continue;
				}

				if (l_neighbourNodes[n].c_parentNode == null) {
					l_neighbourNodes[n].c_parentNode = l_nextNode;
				}

				if (l_neighbourNodes[n].c_gCost > l_neighbourNodes[n].c_parentNode.c_gCost + 1 || !ListContains(l_openList, l_neighbourNodes[n])) {
					l_neighbourNodes[n].c_gCost = l_neighbourNodes[n].c_parentNode.c_gCost++;
					l_neighbourNodes[n].c_hCost = Mathf.Abs (l_neighbourNodes[n].c_nodePosition.x - l_endPos.x) + Mathf.Abs (l_neighbourNodes[n].c_nodePosition.z - l_endPos.z);
					l_neighbourNodes[n].c_fCost = l_neighbourNodes[n].c_gCost + l_neighbourNodes[n].c_hCost;
					l_neighbourNodes[n].c_parentNode = l_nextNode;
					Debug.Log ("Setting Node parent");

					if (!ListContains(l_openList, l_neighbourNodes[n])) {
						l_openList.Add (l_neighbourNodes[n]);
					}
				}
			}
		}
		if (l_foundPath) {
			Debug.Log ("Found path, calling backtrack");
			l_returnArray = CalculateBacktrack(l_startNode, l_endNode);
		}
		return l_returnArray;
	}

	/// <summary>
	/// Calculates the backtrack.
	/// </summary>
	/// <returns>The backtrack.</returns>
	/// <param name="l_startNode">L start node.</param>
	/// <param name="l_endNode">L end node.</param>
	private Vector3[] CalculateBacktrack(Node l_startNode, Node l_endNode){
		List<Node> l_returnPath = new List<Node> ();
		Node l_tempNode = l_endNode;

		Debug.Log ("Backtracking");

		while (l_tempNode != l_startNode) {
			l_returnPath.Add (l_tempNode);
			l_tempNode = l_tempNode.c_parentNode;
		}
		Vector3[] l_finalPath = new Vector3[l_returnPath.Count];

		for (int x = 0; x < l_returnPath.Count; x++) {
			l_finalPath [x] = l_returnPath [x].c_nodePosition;
			Debug.Log ("Node: " + l_finalPath[x]);
		}

		Array.Reverse(l_finalPath);
		return l_finalPath;
	}


	/// <summary>
	/// Moves to next node co.
	/// </summary>
	/// <returns>The to next node co.</returns>
	/// <param name="l_pathToFollow">L path to follow.</param>
	private IEnumerator MoveToNextNodeCo(Vector3[] l_pathToFollow){
		int l_moveToNextDepth = 0;
		Vector3 l_heightOffset = new Vector3 (0, 1, 0);
		while (c_myTrans.position != l_pathToFollow[l_pathToFollow.Length - 1] + l_heightOffset) 
		{
			Debug.Log (l_pathToFollow [l_moveToNextDepth]);
			while (c_myTrans.position != (l_pathToFollow [l_moveToNextDepth] + l_heightOffset)) 
			{
				Vector3 dir = ((l_pathToFollow [l_moveToNextDepth] + l_heightOffset) - c_myTrans.position).normalized;
				c_myTrans.position += dir * 35f * Time.deltaTime;

				if (Mathf.Abs (((l_pathToFollow [l_moveToNextDepth] + l_heightOffset) - c_myTrans.position).magnitude) < 1)
					c_myTrans.position = l_pathToFollow [l_moveToNextDepth] + l_heightOffset;

				yield return null;
			}
			l_moveToNextDepth++;
		}
		StopCoroutine ("MoveToNextNodeCo");
	}

	/// <summary>
	/// Initiates the move.
	/// </summary>
	/// <returns>The move.</returns>
	/// <param name="l_startPos">L start position.</param>
	/// <param name="l_endPos">L end position.</param>
	public string InitiateMove(Vector3 l_startPos, Vector3 l_endPos)
	{
		Vector3[] l_pathToFollow = CalculatePath (l_startPos, l_endPos);
		StartCoroutine(MoveToNextNodeCo(l_pathToFollow));
		return "Finished Moving";
	}
}


