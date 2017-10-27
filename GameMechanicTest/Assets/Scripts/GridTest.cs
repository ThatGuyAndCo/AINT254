﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridTest : MonoBehaviour {

	//Note: this code is just testing code for adapting pre-existing 2D tutorial code
	//to my 3D project. This will be changed as necessary for my own purposes.

	[SerializeField]
	public class Count{
		public int c_min;
		public int c_max;

		public Count(int l_min, int l_max){
			c_min = l_min;
			c_max = l_max;
		}
	}

	public int c_columns;
	public int c_rows;
	public Count c_playerChars = new Count(3, 5);
	public Count c_enemyChars = new Count (3, 5);

	public GameObject c_floor;
	public GameObject c_enemy;
	public GameObject c_player;

	private Transform c_myBoard;

	//Here I use both a 2-dimensional array and a list. The list is used to generate players/enemies/items/terrain etc and remove positions that already have things in that position.
	//The Array however, will be used to move around the terrain later in the player and enemy move functions (as such it is static).
	private List <Vector3> c_gridPositions = new List<Vector3> ();
	public static Vector3[,] s_gridPosArray; //[x,z]


	public static int[] GetArrayPosFromVector(Vector3 l_myPos)
	{
		Debug.Log ("Called GetPosFromVec");
		int[] l_returnArrayPos = new int[2];

		for (int x = 0; x < s_gridPosArray.GetLength (0); x++) 
		{
			Debug.Log ("Testing X @: " + x + ", Local value " + s_gridPosArray [x, 0].x + ". Expected value: " + l_myPos.x);
			if (s_gridPosArray [x, 0].x == l_myPos.x) 
			{
				Debug.Log ("X found @: " + x + " with value local value " + s_gridPosArray [x, 0].x + ". Expected value: " + l_myPos.x);
				l_returnArrayPos [0] = x;
                                                       

				for (int z = 0; z < s_gridPosArray.GetLength (1); z++) 
				{
					Debug.Log ("Testing Z @: " + z + " Local value " + s_gridPosArray [x, z].z + ". Expected value: " + l_myPos.z);
					if (s_gridPosArray [x, z].z == l_myPos.z) {
						l_returnArrayPos [1] = z;
						Debug.Log ("Z found @: " + z + " with value local value " + s_gridPosArray [x, z].z + ". Expected value: " + l_myPos.z);
						return l_returnArrayPos;
					}
				}
			}
		}

		return new int[]{ -100, -100 };
	}

	void InitialiseList(){
		s_gridPosArray = new Vector3[c_columns, c_rows];
		c_gridPositions.Clear ();

		for (int x = 5; x < (c_columns * 10) + 5; x += 10) {
			for (int z = 5; z < (c_rows * 10) + 5; z += 10) {

				int l_testX = (x - 5) / 10;
				l_testX = c_columns - 1 - l_testX;

				int l_testZ = (z - 5) / 10;
				l_testZ = c_rows - 1 - l_testZ;

				Debug.Log ("Array x: " + l_testX + ", Array z: " + l_testZ + ", values: x: " + x + ", z: " + z);
				s_gridPosArray [l_testX, l_testZ] = new Vector3 (x , 0f, z);
				c_gridPositions.Add (new Vector3 (x, 0f, z));
			}
		}
	}

	void BoardSetup(){
		c_myBoard = new GameObject ("Board").transform;

		for (int x = 5; x < (c_columns * 10) + 5; x += 10) {
			for (int z = 5; z < (c_rows * 10) + 5; z += 10) {
				GameObject l_toInstantiate = c_floor;
				GameObject l_instance = Instantiate (l_toInstantiate, new Vector3 (x, 0f, z), Quaternion.identity) as GameObject;
				l_instance.transform.SetParent (c_myBoard);
			}
		}
	}

	void BattleSetup(){
		int l_gridPositionsRemoved = 0;
		int l_players = Random.Range (c_playerChars.c_min, c_playerChars.c_max);
		bool l_mainAssigned = false;

		for (int l_Count = 0; l_Count < l_players; l_Count++) {
			int l_randomIndex = Random.Range (0, c_gridPositions.Count / 2);
			GameObject l_newPlayer = Instantiate (c_player, c_gridPositions [l_randomIndex], Quaternion.identity);
			l_newPlayer.transform.SetParent (c_myBoard);

			if (!l_mainAssigned) {
				l_newPlayer.GetComponent<PlayerAttack> ().c_myTurn = true;
				l_mainAssigned = true;
			}
			
			c_gridPositions.RemoveAt (l_randomIndex);
			l_gridPositionsRemoved++;
		}

		int l_enemies = Random.Range (c_enemyChars.c_min, c_enemyChars.c_max);
		for (int l_Count = 0; l_Count < l_enemies; l_Count++) {
			int l_randomIndex = Random.Range (c_gridPositions.Count / 2 - l_gridPositionsRemoved, c_gridPositions.Count);
			GameObject l_newEnemy = Instantiate (c_enemy, c_gridPositions [l_randomIndex], Quaternion.identity);
			l_newEnemy.transform.SetParent (c_myBoard);
			c_gridPositions.RemoveAt (l_randomIndex);
		}
	}

	void Start(){
		InitialiseList ();
		BoardSetup ();
		BattleSetup ();
	}
}
