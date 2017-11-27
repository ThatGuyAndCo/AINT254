using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMove : MonoBehaviour {

	protected List<Node> FindNeighbours(List<Node> l_openList, List<Node> l_closedList, Node l_currentNode){
		int[] l_startGrid = GridTest.GetArrayPosFromVector (l_currentNode.c_nodePosition);
		//Debug.Log (l_startGrid[0] + ", " + l_startGrid[1] + " is grid pos, current node transform = " + l_currentNode.c_nodePosition);
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

	protected bool IsThereObstruction (Vector3 l_node)
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

	protected bool ListContains(List<Node> l_searchList, Node l_searchForNode){
		for (int i = 0; i < l_searchList.Count; i++) {
			if (l_searchList [i] == l_searchForNode) 
				return true;
		}
		return false;
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
