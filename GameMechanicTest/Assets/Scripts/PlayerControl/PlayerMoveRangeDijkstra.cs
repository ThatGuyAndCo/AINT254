using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveRangeDijkstra : AbstractMove {

	/// <summary>
	/// This method uses a Dijkstra algoritm to create a list of all floor tiles within the argument's range.
	/// I have used for loops as opposed to foreach loops for efficiency and created my own List.Contains()
	/// method that loops through the list to find a comparable node as this too is much more efficient.
	/// </summary>
	/// <returns>GameObject[] l_returnArea: List of game objects in range of the player.</returns>
	/// <param name="l_startPos">: Player's position in the world (Vector3).</param>
	/// <param name="l_range">: Range that player can move (int).</param>
	private GameObject[] CalculateArea(Vector3 l_startPos, int l_range){
		List<Node> l_areaInRange = new List<Node>();

		List<Node> l_openList = new List<Node>();
		List<Node> l_closedList = new List<Node>();
		int[] l_tempGrid;
		l_tempGrid = GridTest.GetArrayPosFromVector (l_startPos);
		Node l_startNode = GridTest.s_gridPosArray [l_tempGrid[0], l_tempGrid[1]];
		l_openList.Add (l_startNode);

		while (l_openList.Count > 0) {
			Node l_nextNode = new Node(new Vector3(0,0,0));
			l_nextNode.c_gCost = int.MaxValue;

			for (int n = 0; n < l_openList.Count; n++) {
				if(l_openList[n].c_gCost < l_nextNode.c_gCost)
					l_nextNode = l_openList[n];
			}

			if (l_nextNode == null)
				break;

			l_openList.Remove (l_nextNode);
			l_closedList.Add (l_nextNode);

			if (l_nextNode.c_gCost > l_range)
				continue;

			List<Node> l_neighbourNodes = FindNeighbours (l_openList, l_closedList, l_nextNode);

			for (int n = 0; n < l_neighbourNodes.Count; n++) {
				if (IsThereObstruction (l_neighbourNodes[n].c_nodePosition) || ListContains(l_closedList, l_neighbourNodes[n])) {
					continue;
				}
				if(l_neighbourNodes[n].c_parentNode == null || !ListContains(l_closedList, l_neighbourNodes[n].c_parentNode))
					l_neighbourNodes[n].c_parentNode = l_nextNode;

				if ((l_neighbourNodes[n].c_gCost > (l_neighbourNodes[n].c_parentNode.c_gCost + 1) || l_neighbourNodes[n].c_gCost == 0) || !ListContains(l_openList, l_neighbourNodes[n])) {
					l_neighbourNodes[n].c_gCost = l_neighbourNodes[n].c_parentNode.c_gCost + 1;
					l_neighbourNodes[n].c_fCost = l_neighbourNodes[n].c_gCost;
					l_neighbourNodes[n].c_parentNode = l_nextNode;

					if (!ListContains(l_openList, l_neighbourNodes[n]) && l_neighbourNodes[n].c_gCost <= l_range) {
						l_openList.Add (l_neighbourNodes[n]);
						Debug.Log ("GCost = " + l_neighbourNodes [n].c_gCost + " added to openList");
						if (!ListContains(l_areaInRange, l_neighbourNodes[n])) {
							l_areaInRange.Add (l_neighbourNodes[n]);
							Debug.Log ("GCost = " + l_neighbourNodes [n].c_gCost + " added to areaInRangeList");
						}
					}
				}
			}
		}

		List<GameObject> l_returnArea = new List<GameObject> ();

		foreach (Node n in l_areaInRange) {
			Debug.Log (n.c_nodePosition);
		}

		for (int n = 0; n < l_areaInRange.Count; n++) {
			l_areaInRange [n].c_gCost = 0;
			l_areaInRange [n].c_fCost = 0;
			l_areaInRange [n].c_parentNode = null;
			GameObject l_tempObj = GridTest.GetTileFromVector (l_areaInRange[n].c_nodePosition);
			if (l_tempObj != null) {
				l_returnArea.Add (l_tempObj);
			}
		}

		return l_returnArea.ToArray();
	}	

	/// <summary>
	/// This is an accessor method to reduce the amount of code available publicly and increase encapsulation
	/// </summary>
	/// <returns>GameObject[] l_returnNodesInRange: The nodes the player can travel to.</returns>
	/// <param name="l_startPos">: Player's position in the world (Vector3).</param>
	/// <param name="l_range">: Range that player can move (int).</param>
	public GameObject[] FindMoveArea(Vector3 l_startPos, int l_range)
	{
		Debug.Log ("Range = " + l_range);
		GameObject[] l_returnNodesInRange = CalculateArea (l_startPos, l_range);
		return l_returnNodesInRange;
	}
}
