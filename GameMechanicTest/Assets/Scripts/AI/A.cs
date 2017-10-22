using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class A : MonoBehaviour {
	
	Grid grid;
	PathRequestManager requestManager; 

	void Awake(){
		grid = GetComponent<Grid> ();
		requestManager = GetComponent<PathRequestManager> ();
	}

	IEnumerator FindPath (Vector3 startPos, Vector3 endPos){
		Vector3[] wayPoints = new Vector3 [0];
		bool pathSuccess = false;
		Node startNode = grid.NodeFromWorld (startPos);
		Node targetNode = grid.NodeFromWorld (endPos);

		if (startNode.walkable && targetNode.walkable) {
			Heap<Node> openList = new Heap<Node> (grid.MaxSize);
			HashSet<Node> closedList = new HashSet<Node> ();

			openList.Add (startNode);

			while (openList.Count > 0) {
				Node currentNode = openList.RemoveFirst ();
				closedList.Add (currentNode);

				if (currentNode == targetNode) {
					pathSuccess = true;
					break;
				}

				foreach (Node neighbour in grid.getNeighbours(currentNode)) {
					if (!neighbour.walkable || closedList.Contains (neighbour)) {
						continue;
					}

					int newCostToNeighbour = currentNode.gCost + getDistance (currentNode, neighbour) + neighbour.movePenalty;

					if (newCostToNeighbour < neighbour.gCost || !openList.Contains (neighbour)) {
						neighbour.gCost = newCostToNeighbour;
						neighbour.hCost = getDistance (neighbour, targetNode);
						neighbour.parent = currentNode;

						if (!openList.Contains (neighbour)) {
							openList.Add (neighbour);
						} else {
							openList.UpdateItem (neighbour);
						}
					}
				}
			}
		}
		yield return null;
		if (pathSuccess) {
			wayPoints = retracePath (startNode, targetNode);
		}
		requestManager.FinishedProcessingPath (wayPoints, pathSuccess);
	}

	public void StartFindPath(Vector3 startPos, Vector3 targetPos){
		StartCoroutine(FindPath(startPos, targetPos));
	}

	Vector3[] retracePath(Node start, Node end){
		List<Node> path = new List<Node> ();
		Node currentNode = end;

		while (currentNode != start) {
			path.Add (currentNode);
			currentNode = currentNode.parent;
		}
		Vector3[] waypoints = SimplifyPath (path);
		Array.Reverse(waypoints);
		return waypoints;
	}

	Vector3[] SimplifyPath(List<Node> path){
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 directionOld = Vector2.zero;

		for(int i = 1; i < path.Count; i++){
			Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridZ - path[i].gridZ);
			if(directionNew != directionOld){
				waypoints.Add(path[i].nodePosition);
			}
			directionOld = directionNew;
		}

		return waypoints.ToArray ();
	}

	public int getDistance(Node nodeA, Node nodeB){
		int distX = Mathf.Abs (nodeA.gridX - nodeB.gridX);
		int distY = Mathf.Abs (nodeA.gridZ - nodeB.gridZ);

		if (distX > distY) {
			return (14 * distY) + (10 * (distX - distY));
		} else {
			return (14 * distX) + (10 * (distY - distX));
		}
	}
}
