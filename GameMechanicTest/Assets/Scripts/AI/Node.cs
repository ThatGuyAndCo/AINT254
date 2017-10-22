using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>{

	public bool walkable;
	public Vector2 nodePosition;
	public int gCost;
	public int hCost;
	public int gridX;
	public int gridZ;
	public Node parent;
	int heapIndex;

	public int movePenalty;

	public Node(bool _walkable, Vector2 _nodePosition, int _gridX, int _gridZ, int _penalty){
		walkable = _walkable;
		nodePosition = _nodePosition;
		gridX = _gridX;
		gridZ = _gridZ;
		movePenalty = _penalty;
	}

	public int fCost{
		get{
			return hCost + gCost;
		}
	}

	public int HeapIndex {
		get{
			return heapIndex;
		}
		set{
			heapIndex = value;
		}
	}

	public int CompareTo(Node nodeToCompare){
		int compare = fCost.CompareTo(nodeToCompare.fCost);

		if (compare == 0) {
			compare = hCost.CompareTo (nodeToCompare.hCost);
		}
		return -compare;


	}
}
