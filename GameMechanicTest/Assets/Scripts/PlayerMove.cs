using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : AbstractMove {

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
			l_horizontal = -1;
		else if (l_startPos.x > l_endPos.x)
			l_horizontal = 1;
		if (l_startPos.z < l_endPos.z)
			l_vertical = -1;
		else if (l_startPos.z > l_endPos.z)
			l_vertical = 1;

		c_myGridPos = GridTest.GetArrayPosFromVector (l_startPos);
		Debug.Log (c_myGridPos[0] + ", " + c_myGridPos[1]);

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
			Debug.Log (l_newNode);
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
		if (l_node == new Vector3 (-100, -100, -100)) {
			Debug.Log("Obstruction Found @ " + l_node);
			return true;
		}

		if (Physics.Raycast (l_node, Vector3.up, 10, 8) || Physics.Raycast (l_node, Vector3.up, 10, 9)) {
			Debug.Log("Obstruction Found @ " + l_node);
			return true;
		}

		Debug.Log("Obstruction Not Found @ " + l_node);
		return false;
	}

	private IEnumerator MoveToNextNodeCo(Vector3[] l_pathToFollow){
		int l_moveToNextDepth = 0;
		Debug.Log (l_pathToFollow.Length);

		for (int a = 0; a < l_pathToFollow.Length - 1; a++) {
			Debug.Log (a);
			Debug.Log ("CoRoutine Loop: " + a + ", " + l_pathToFollow [a]);
		}

		while (c_myTrans.position != l_pathToFollow[l_pathToFollow.Length - 1]) {
			Debug.Log ("Going down");
			while (c_myTrans.position != l_pathToFollow [l_moveToNextDepth]) {
				//c_myTrans.position = Vector3.MoveTowards (c_myTrans.position, l_pathToFollow [l_moveToNextDepth], 2000f * Time.deltaTime);
				Vector3 dir = (l_pathToFollow [l_moveToNextDepth] - c_myTrans.position).normalized;
				c_myTrans.position += dir * 35f * Time.deltaTime;

				if (Mathf.Abs ((l_pathToFollow [l_moveToNextDepth] - c_myTrans.position).magnitude) < 1)
					c_myTrans.position = l_pathToFollow [l_moveToNextDepth];

				
				
				yield return null;
			}
			l_moveToNextDepth++;

			//yield return new WaitForSeconds(2.1f);
			//yield return null;
		}
		Debug.Log ("Stopping Co-routine");
		StopCoroutine ("MoveToNextNodeCo");
	}

//	protected override void MoveToNextNode()
//	{
//		Debug.Log (c_pathToFollow.Length);
//		//c_myTrans.position = Vector3.MoveTowards (c_myTrans.position, c_pathToFollow [c_moveToNextDepth], 1f * Time.deltaTime);
//		StartCoroutine(MoveToNextNodeCo());
//		c_moveToNextDepth++;
//		if (c_myTrans.position != c_pathToFollow[c_pathToFollow.Length + 1]) {
//			Debug.Log ("Going down");
//			Invoke("MoveToNextNode", 1.5f);
//		}
//		Debug.Log ("Going Up");
//	}

	protected override bool IsTurnOver(Vector3 l_startPos, Vector3 l_endPos)
	{
		return false;
	}

	public override string InitiateMove(Vector3 l_startPos, Vector3 l_endPos, bool l_playerAttacked)
	{
		Debug.Log ("Called Initiate move");
		Debug.Log ("Calling Calc Path");
		c_pathToFollow = CalculatePath (l_startPos, l_endPos);
		StartCoroutine(MoveToNextNodeCo(c_pathToFollow));
		Debug.Log ("Finished Moving");
		return "moved";
	}

}
