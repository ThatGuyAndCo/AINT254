using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMove : MonoBehaviour {

	/// <summary>
	/// This is a helper method for the pathfnding that searches the grid positions adjacent to the current node to determine if the node is in the grid (using the try/catch)
	/// and that the open/closed lists do not already contain the node (as this would create an infinite loop)
	/// </summary>
	/// <returns>List<Node> l_returnNodes (A list of nodes adjacent to the current node)</returns>
	/// <param name="l_openList">The pathfinding algorithm's open list</param>
	/// <param name="l_closedList">The pathfinding algorithm's closed list</param>
	/// <param name="l_currentNode">The node to find adjecant nodes to</param>
	protected List<Node> FindNeighbours(List<Node> l_openList, List<Node> l_closedList, Node l_currentNode){
		int[] l_startGrid = GridTest.GetArrayPosFromVector (l_currentNode.c_nodePosition);
		//Debug.Log (l_startGrid[0] + ", " + l_startGrid[1] + " is grid pos, current node transform = " + l_currentNode.c_nodePosition);
		List<Node> l_returnNodes = new List<Node>();

		Node l_tempNode = new Node (new Vector3 (0, 0, 0));

		try{
			l_tempNode = GridTest.s_gridPosArray [l_startGrid [0] + 1, l_startGrid [1]];
			if (!ListContains(l_openList, l_tempNode) && !ListContains(l_closedList, l_tempNode)) {
				l_returnNodes.Add (l_tempNode);
			}
		}
		catch{
		}
		try{
			l_tempNode = GridTest.s_gridPosArray [l_startGrid [0] - 1, l_startGrid [1]];
			if (!ListContains(l_openList, l_tempNode) && !ListContains(l_closedList, l_tempNode)) {
				l_returnNodes.Add (l_tempNode);
			}
		}
		catch{
		}
		try{
			l_tempNode = GridTest.s_gridPosArray [l_startGrid [0], l_startGrid [1] + 1];
			if (!ListContains(l_openList, l_tempNode) && !ListContains(l_closedList, l_tempNode)) {
				l_returnNodes.Add (l_tempNode);
			}
		}
		catch{
		}
		try{
			l_tempNode = GridTest.s_gridPosArray [l_startGrid [0], l_startGrid [1] - 1];
			if (!ListContains(l_openList, l_tempNode) && !ListContains(l_closedList, l_tempNode)) {
				l_returnNodes.Add (l_tempNode);
			}
		}
		catch{
		}

		return l_returnNodes;
	}

	/// <summary>
	/// Raycast's the node's position to determine if there is an obstruction.
	/// </summary>
	/// <returns><c>true</c> If there is an obstruction the specified node; otherwise, <c>false</c>.</returns>
	/// <param name="l_node">The node to be searched</param>
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

	/// <summary>
	/// This is a helper method to hand search though a node list to see if a specific node is in the list.
	/// This is significantly faster than using the List.Contains() method and dramatically improved performance of the Dijkstra's pathfinding.
	/// </summary>
	/// <returns><c>true</c>, If the node is in the list, <c>false</c> otherwise.</returns>
	/// <param name="l_searchList">The list to search</param>
	/// <param name="l_searchForNode">The node to search for</param>
	protected bool ListContains(List<Node> l_searchList, Node l_searchForNode){
		for (int i = 0; i < l_searchList.Count; i++) {
			if (l_searchList [i] == l_searchForNode) 
				return true;
		}
		return false;
	}
}

/// <summary>
/// Node class to contain information used by the pathfinding algorithms.
/// </summary>
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
