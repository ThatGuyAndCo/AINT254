using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : AbstractMove {

	private int c_moveToNextDepth;

	private Vector3[] c_pathToFollow;

	private int[] c_myGridPos;

	private Transform c_myTrans;

	void Start(){
		c_myTrans = transform;
	}

	protected override Vector3 CalculateMove (Vector3 l_startPos, Vector3 l_endPos, Vector3 l_ignoreNode)
	{
		int l_horizontal = 0;
		int l_vertical = 0;
		Vector3 l_nextNode = new Vector3();

		if (l_startPos.x < l_endPos.x)
			l_horizontal = 1;
		else if (l_startPos.x > l_endPos.x)
			l_horizontal = -1;
		if (l_startPos.z < l_endPos.z)
			l_vertical = 1;
		else if (l_startPos.z > l_endPos.z)
			l_vertical = -1;

		c_myGridPos = GridTest.GetArrayPosFromVector (l_startPos);

		Debug.Log (c_myGridPos);

		bool l_foundNext = false;
		while (true)
		{
			if (l_horizontal == 1) 
			{
				l_nextNode = GridTest.s_gridPosArray [c_myGridPos [0] + 1, c_myGridPos [1]];

				if (l_nextNode == l_ignoreNode) 
					l_nextNode = new Vector3 (-100, -100, -100);

				l_foundNext = !IsThereObstruction (l_nextNode);

				if (l_foundNext)
					break;
			}
			if (l_horizontal == -1) 
			{
				l_nextNode = GridTest.s_gridPosArray [c_myGridPos [0] + -1, c_myGridPos [1]];

				if (l_nextNode == l_ignoreNode) 
					l_nextNode = new Vector3 (-100, -100, -100);

				l_foundNext = !IsThereObstruction (l_nextNode);

				if (l_foundNext)
					break;
			}
			if (l_vertical == 1) 
			{
				l_nextNode = GridTest.s_gridPosArray [c_myGridPos [0], c_myGridPos [1] + 1];

				if (l_nextNode == l_ignoreNode) 
					l_nextNode = new Vector3 (-100, -100, -100);

				l_foundNext = !IsThereObstruction (l_nextNode);

				if (l_foundNext)
					break;
			}
			if (l_vertical == -1) 
			{
				l_nextNode = GridTest.s_gridPosArray [c_myGridPos [0], c_myGridPos [1] - 1];

				if (l_nextNode == l_ignoreNode) 
					l_nextNode = new Vector3 (-100, -100, -100);

				l_foundNext = !IsThereObstruction (l_nextNode);
			}
			break;
		}

		return l_nextNode;
	}
		
	protected override Vector3[] CalculatePath(Vector3 l_startPos, Vector3 l_endPos)
	{
		Debug.Log ("Called calc path");
		List<Vector3> pathNodes = new List<Vector3>();
		Vector3 l_ignoreNode = new Vector3(-100, -100, -100);
		pathNodes.Add (l_startPos);
		while (true) 
		{
			Debug.Log ("Looping path");
			Debug.Log (l_startPos + " to " + l_endPos);
			if (l_startPos == l_endPos)
				break;
			
			Vector3 l_newNode = CalculateMove (l_startPos, l_endPos, l_ignoreNode);
			if (l_newNode != new Vector3 (-100, -100, -100)) 
			{
				pathNodes.Add (l_newNode);
				l_startPos = l_newNode;
			} 
			else 
			{
				l_ignoreNode = l_startPos;
				pathNodes.Remove (l_startPos);
				l_startPos = pathNodes [pathNodes.Count + 1];
			}
		}

		return pathNodes.ToArray();
	}

	protected override bool IsThereObstruction (Vector3 l_node)
	{
		if (l_node == new Vector3 (-100, -100, -100))
			return true;

		if (Physics.Raycast (l_node, Vector3.up, 10, 8) || Physics.Raycast (l_node, Vector3.up, 10, 9))
			return true;

		return false;
	}


	protected override void MoveToNextNode()
	{
		c_myTrans.position = Vector3.MoveTowards (c_myTrans.position, c_pathToFollow [c_moveToNextDepth], 1f * Time.deltaTime);
		c_moveToNextDepth++;
		if (c_myTrans.position != c_pathToFollow[c_pathToFollow.Length + 1]) {
			Debug.Log ("Going down");
			Invoke("MoveToNextNode", 1.5f);
		}
		Debug.Log ("Going Up");
	}

	protected override bool IsTurnOver(Vector3 l_startPos, Vector3 l_endPos)
	{
		return false;
	}

	public override string InitiateMove(Vector3 l_startPos, Vector3 l_endPos, bool l_playerAttacked)
	{
		Debug.Log ("Called Initiate move");
		c_moveToNextDepth = 0;
		Debug.Log ("Calling Calc Path");
		c_pathToFollow = CalculatePath (l_startPos, l_endPos);
		MoveToNextNode ();
		return "moved";
	}

}
