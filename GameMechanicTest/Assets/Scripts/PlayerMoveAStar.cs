using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMoveAStar : MonoBehaviour {

	private Vector3[] c_pathToFollow;

	private int[] c_myGridPos;

	private Transform c_myTrans;

	List<Vector3> c_ignoreNodes;

	void Start(){
		c_myTrans = transform;
	}

	private List<Node> FindNeighbours(List<Node> l_openList, List<Node> l_closedList, Node l_currentNode){
		int[] l_startGrid = GridTest.GetArrayPosFromVector (l_currentNode.c_nodePosition);
		Debug.Log (l_startGrid[0] + ", " + l_startGrid[1] + " is grid pos, current node transform = " + l_currentNode.c_nodePosition);
		List<Node> l_returnNodes = new List<Node>();

		Node l_tempNode = new Node (new Vector3 (0, 0, 0));

		try{
			l_tempNode = GridTest.s_gridPosArray [l_startGrid [0] + 1, l_startGrid [1]];
			if (!l_openList.Contains (l_tempNode) && !l_closedList.Contains (l_tempNode)) {
				l_returnNodes.Add (l_tempNode);
			}
		}
		catch{
			
		}

		try{
			l_tempNode = GridTest.s_gridPosArray [l_startGrid [0] - 1, l_startGrid [1]];
			if (!l_openList.Contains (l_tempNode) && !l_closedList.Contains (l_tempNode)) {
				l_returnNodes.Add (l_tempNode);
			}
		}
		catch{

		}

		try{
			l_tempNode = GridTest.s_gridPosArray [l_startGrid [0], l_startGrid [1] + 1];
			if (!l_openList.Contains (l_tempNode) && !l_closedList.Contains (l_tempNode)) {
				l_returnNodes.Add (l_tempNode);
			}
		}
		catch{

		}

		try{
			l_tempNode = GridTest.s_gridPosArray [l_startGrid [0], l_startGrid [1] - 1];
			if (!l_openList.Contains (l_tempNode) && !l_closedList.Contains (l_tempNode)) {
				l_returnNodes.Add (l_tempNode);
			}
		}
		catch{

		}

		return l_returnNodes;
	}

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


			foreach (Node n in l_openList) {
				if (n.c_fCost < l_nextNode.c_fCost)
					l_nextNode = n;
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
			foreach (Node n in l_neighbourNodes) {
				if (IsThereObstruction (n.c_nodePosition) || l_closedList.Contains (n)) {
					continue;
				}

				if (n.c_parentNode == null) {
					n.c_parentNode = l_nextNode;
				}

				if (n.c_gCost > n.c_parentNode.c_gCost + 1 || !l_openList.Contains (n)) {
					n.c_gCost = n.c_parentNode.c_gCost++;
					n.c_hCost = Mathf.Abs (n.c_nodePosition.x - l_endPos.x) + Mathf.Abs (n.c_nodePosition.z - l_endPos.z);
					n.c_fCost = n.c_gCost + n.c_hCost;
					n.c_parentNode = l_nextNode;
					Debug.Log ("Setting Node parent");

					if (!l_openList.Contains (n)) {
						l_openList.Add (n);
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

	private bool IsThereObstruction (Vector3 l_node)
	{
		//Debug.Log ("Obtained " + l_node + " as test obstruction node");
		RaycastHit hit;
		Physics.Raycast (l_node + new Vector3(0, 50, 0), -Vector3.up, out hit, 60f);
		//Debug.DrawRay (l_node + new Vector3(0, 50, 0), -Vector3.up * 50, Color.blue, 10f);

		if (hit.collider != null && !hit.collider.CompareTag("MoveCube")) {
			//Debug.Log ("Ray hit: " + hit.collider.gameObject.name);
			//Debug.Log("Obstruction Found @ " + l_node);
			return true;
		}

		if (l_node == new Vector3 (-100, -100, -100)) 
		{
			//Debug.Log("Obstruction Found @ " + l_node);
			return true;
		}

		//Debug.Log("Obstruction Not Found @ " + l_node);
		return false;
	}

	private IEnumerator MoveToNextNodeCo(Vector3[] l_pathToFollow){
		int l_moveToNextDepth = 0;

		while (c_myTrans.position != l_pathToFollow[l_pathToFollow.Length - 1]) 
		{
			Debug.Log (l_pathToFollow [l_moveToNextDepth]);
			while (c_myTrans.position != l_pathToFollow [l_moveToNextDepth]) 
			{
				Vector3 dir = (l_pathToFollow [l_moveToNextDepth] - c_myTrans.position).normalized;
				c_myTrans.position += dir * 35f * Time.deltaTime;

				if (Mathf.Abs ((l_pathToFollow [l_moveToNextDepth] - c_myTrans.position).magnitude) < 1)
					c_myTrans.position = l_pathToFollow [l_moveToNextDepth];

				yield return null;
			}
			l_moveToNextDepth++;
		}
		StopCoroutine ("MoveToNextNodeCo");
	}

	public string InitiateMove(Vector3 l_startPos, Vector3 l_endPos)
	{
		//Debug.Log ("Called Initiate move");
		//Debug.Log ("Calling Calc Path");
		c_pathToFollow = CalculatePath (l_startPos, l_endPos);
		StartCoroutine(MoveToNextNodeCo(c_pathToFollow));
		//Debug.Log ("Finished Moving");
		return "Finished Moving";
	}
}

public class Node{
	public Vector3 c_nodePosition;
	public int c_gCost = 0;
	public float c_hCost = 0;
	public float c_fCost = 0;
	public Node c_parentNode = null;

	public Node(Vector3 l_nodePosition){
		c_nodePosition = l_nodePosition;
	}
}
