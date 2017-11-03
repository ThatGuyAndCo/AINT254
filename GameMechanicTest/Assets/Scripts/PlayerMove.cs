using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : AbstractMove {

	private Vector3[] c_pathToFollow;

	private int[] c_myGridPos;

	private Transform c_myTrans;

	List<Vector3> c_ignoreNodes;

	void Start(){
		c_myTrans = transform;
	}

	protected override Vector3 CalculateMove (Vector3 l_startPos, Vector3 l_endPos, List<Vector3> l_ignoreNode)
	{
		Debug.Log ("Starting Calc Move with: " + l_startPos);

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

		bool l_foundNext = false;
		Debug.Log ("Horizontal: " + l_horizontal + ", Vertical: " + l_vertical);
		int loopVal = 0;
		while (true)
		{
			if (l_horizontal == 1) 
			{
				Debug.Log ("Entered l_horizontal +1");
				l_nextNode = GridTest.s_gridPosArray [c_myGridPos [0] + 1, c_myGridPos [1]];
				if (l_ignoreNode.Contains(l_nextNode)) 
					l_nextNode = new Vector3 (-100, -100, -100);

				Debug.Log ("Next node passed to Obstruction: " + l_nextNode);
				l_foundNext = !IsThereObstruction (l_nextNode);
				Debug.Log (l_foundNext);
				if (l_foundNext) {
					Debug.Log ("Broke @ hoz + 1, next node found");
					break;
				}
			}
			if (l_horizontal == -1) 
			{
				Debug.Log ("Entered l_horizontal -1");
				l_nextNode = GridTest.s_gridPosArray [c_myGridPos [0] + -1, c_myGridPos [1]];

				if (l_ignoreNode.Contains(l_nextNode)) 
					l_nextNode = new Vector3 (-100, -100, -100);

				Debug.Log ("Next node passed to Obstruction: " + l_nextNode);
				l_foundNext = !IsThereObstruction (l_nextNode);
				Debug.Log (l_foundNext);
				if (l_foundNext) {
					Debug.Log ("Broke @ hoz - 1, next node found");
					break;
				}
			}
			if (l_vertical == 1) 
			{
				Debug.Log ("Entered l_vertical +1");
				l_nextNode = GridTest.s_gridPosArray [c_myGridPos [0], c_myGridPos [1] + 1];

				if (l_ignoreNode.Contains(l_nextNode)) 
					l_nextNode = new Vector3 (-100, -100, -100);

				Debug.Log ("Next node passed to Obstruction: " + l_nextNode);
				l_foundNext = !IsThereObstruction (l_nextNode);
				Debug.Log (l_foundNext);
				if (l_foundNext) {
					Debug.Log ("Broke @ vert + 1, next node found");
					break;
				}
			}
			if (l_vertical == -1) 
			{
				Debug.Log ("Entered l_vertical -1");
				l_nextNode = GridTest.s_gridPosArray [c_myGridPos [0], c_myGridPos [1] - 1];

				if (l_ignoreNode.Contains(l_nextNode)) 
					l_nextNode = new Vector3 (-100, -100, -100);

				Debug.Log ("Next node passed to Obstruction: " + l_nextNode);
				l_foundNext = !IsThereObstruction (l_nextNode);
				Debug.Log (l_foundNext);
			}

			Debug.Log ("Printing ignoreList");
			foreach (Vector3 node in l_ignoreNode) {
				Debug.Log (node);
			}
			Debug.Log ("Finished printing ignoreList");


			if (!l_ignoreNode.Contains (l_nextNode)) {
				Debug.Log ("Ignore list doesn't contain next node, next node found");
				Debug.Log ("Ignore list !contains " + l_nextNode);
				break;
			}
			else 
			{
				if (l_vertical == 0)
					l_vertical = 1;
				
				if (l_horizontal == 0)
					l_horizontal = 1;

				if (loopVal == 1) {
					if (l_vertical == 1)
						l_vertical = -1;

					if (l_horizontal == 1)
						l_horizontal = -1;
				}
				loopVal++;
			}

			if (loopVal > 3)
				break;
		}

		if (!l_foundNext) {
			c_ignoreNodes.Add(l_nextNode);
			l_nextNode = new Vector3 (-100, -100, -100);
		}

		return l_nextNode;
	}
		
	protected override Vector3[] CalculatePath(Vector3 l_startPos, Vector3 l_endPos)
	{
		List<Vector3> l_pathNodes = new List<Vector3>();
		c_ignoreNodes = new List<Vector3>();
		Vector3 l_saveStartPos = l_startPos;
		l_pathNodes.Add (l_startPos);

		int loopStopper = 0;
		c_ignoreNodes.Add(new Vector3(-100,-100,-100));
		c_ignoreNodes.Add(l_startPos);

		while (l_startPos != l_endPos) 
		{
			Vector3 l_newNode = CalculateMove (l_startPos, l_endPos, c_ignoreNodes);


			if (l_newNode != new Vector3 (-100, -100, -100)) 
			{
				Debug.Log ("New node is not dead value");
				l_pathNodes.Add (l_newNode);
				l_startPos = l_newNode;
			} 
			else 
			{
				Debug.Log ("New node is dead value");

				Debug.Log ("Start node = " + l_startPos);


				for (int x = 0; x < l_pathNodes.Count; x++) 
				{
					Debug.Log(l_pathNodes [x]);
				}

				l_pathNodes.Remove (l_startPos);
				Debug.Log ("Removed l_startPos from list");

				if (l_pathNodes.Count > 2) 
				{
					Debug.Log ("Start node = " + l_startPos);
					l_startPos = l_pathNodes [l_pathNodes.Count - 1];
				} 
				else 
				{
					l_startPos = l_saveStartPos;
				}
			}
			if(!c_ignoreNodes.Contains(l_startPos))
				c_ignoreNodes.Add(l_startPos);
			
			loopStopper++;

			if (loopStopper > 100) {
				Debug.Log ("Loop broken");
				break;
			}
		}

		return l_pathNodes.ToArray();
	}

	protected override bool IsThereObstruction (Vector3 l_node)
	{
		Debug.Log ("Obtained " + l_node + " as test obstruction node");
		RaycastHit hit;
		Physics.Raycast (l_node, Vector3.up, out hit, 100f);
		Debug.DrawRay (l_node + new Vector3(0, 50, 0), Vector3.up * 100, Color.blue, 10f);

		if (hit.collider != null && !hit.collider.CompareTag("MoveCube")) {
			Debug.Log ("Ray hit: " + hit.collider.gameObject.name);
			Debug.Log("Obstruction Found @ " + l_node);
			return true;
		}

		if (l_node == new Vector3 (-100, -100, -100)) 
		{
			Debug.Log("Obstruction Found @ " + l_node);
			return true;
		}

		Debug.Log("Obstruction Not Found @ " + l_node);
		return false;
	}

	protected override IEnumerator MoveToNextNodeCo(Vector3[] l_pathToFollow){
		int l_moveToNextDepth = 0;

		while (c_myTrans.position != l_pathToFollow[l_pathToFollow.Length - 1]) 
		{
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

	public override string InitiateMove(Vector3 l_startPos, Vector3 l_endPos)
	{
		Debug.Log ("Called Initiate move");
		Debug.Log ("Calling Calc Path");
		c_pathToFollow = CalculatePath (l_startPos, l_endPos);
		StartCoroutine(MoveToNextNodeCo(c_pathToFollow));
		Debug.Log ("Finished Moving");
		return "Finished Moving";
	}

}
