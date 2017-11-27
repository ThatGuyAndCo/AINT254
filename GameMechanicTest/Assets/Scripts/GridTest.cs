using System.Collections;
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
	public Count c_playerChars = new Count(6, 8);
	public Count c_enemyChars = new Count (6, 8);

	public GameObject c_floor;
	public GameObject c_wall;
	public GameObject[] c_enemies;
	public GameObject[] c_players;

	private Transform c_myBoard;

	//Here I use both a 2-dimensional array and a list. The list is used to generate players/enemies/items/terrain etc and remove positions that already have things in that position.
	//The Array however, will be used to move around the terrain later in the player and enemy move functions (as such it is static).
	private List <Vector3> c_gridPositions = new List<Vector3> ();
	public static Node[,] s_gridPosArray; //[x,z]

	public static List<GameObject> s_playerCharacters = new List<GameObject>(); 
	public static List<GameObject> s_enemyCharacters = new List<GameObject>(); 


	public static int[] GetArrayPosFromVector(Vector3 l_myPos)
	{
		int[] l_returnArrayPos = new int[2];

		for (int x = 0; x < s_gridPosArray.GetLength (0); x++) 
		{
			if (s_gridPosArray [x, 0].c_nodePosition.x == l_myPos.x) 
			{
				l_returnArrayPos [0] = x;
                                                       

				for (int z = 0; z < s_gridPosArray.GetLength (1); z++) 
				{
					if (s_gridPosArray [x, z].c_nodePosition.z == l_myPos.z) {
						l_returnArrayPos [1] = z;
						return l_returnArrayPos;
					}
				}
			}
		}

		return new int[]{ -100, -100 };
	}

	public static GameObject GetTileFromVector(Vector3 l_node){
		//Debug.Log ("Obtained " + l_node + " as test obstruction node");
		RaycastHit l_hit;
		int l_moveLayerMask = 1 << 10;
		Physics.Raycast (l_node + new Vector3(0, 50, 0), -Vector3.up, out l_hit, 60f, l_moveLayerMask);
		Debug.DrawRay (l_node + new Vector3(0, 50, 0), -Vector3.up * 60, Color.blue, 10f);

		if (l_hit.collider != null && l_hit.collider.CompareTag("MoveCube")) {
			return l_hit.collider.gameObject;
		}

		Debug.Log ("GridTest GetTileFromVector returning null");
		return null;
	}

	void InitialiseList(){
		s_gridPosArray = new Node[c_columns, c_rows];
		c_gridPositions.Clear ();

		for (int x = 5; x < (c_columns * 10) + 5; x += 10) {
			for (int z = 5; z < (c_rows * 10) + 5; z += 10) {

				int l_testX = (x - 5) / 10;
				l_testX = c_columns - 1 - l_testX;

				int l_testZ = (z - 5) / 10;
				l_testZ = c_rows - 1 - l_testZ;

				s_gridPosArray [l_testX, l_testZ] = new Node(new Vector3 (x , -1f, z));
				c_gridPositions.Add (new Vector3 (x, -1f, z));
			}
		}
	}

	void BoardSetup(){
		c_myBoard = new GameObject ("Board").transform;
		GameObject l_toInstantiate = c_floor;

		for (int x = 5; x < (c_columns * 10) + 5; x += 10) {
			for (int z = 5; z < (c_rows * 10) + 5; z += 10) {
				if (x == 5 || z == 5 || x == (c_columns * 10) - 5 || z == (c_rows * 10) - 5) {
					l_toInstantiate = c_wall;
					c_gridPositions.Remove(new Vector3 (x, -1f, z));
				}
				else
					l_toInstantiate = c_floor;
				
				GameObject l_instance = Instantiate (l_toInstantiate, new Vector3 (x, -1f, z), Quaternion.identity) as GameObject;
				l_instance.transform.SetParent (c_myBoard);
			}
		}
	}

	void BattleSetup(){
		int l_gridPositionsRemoved = 0;
		int l_players = Random.Range (c_playerChars.c_min, c_playerChars.c_max);
		bool l_mainAssigned = false;

		Vector3 l_verticalOffset = new Vector3(0,1,0);

		for (int l_Count = 0; l_Count < l_players; l_Count++) {
			int l_randomIndex = Random.Range (0, c_gridPositions.Count / 2);
			int l_randomGO = Random.Range (0, c_players.Length);
			GameObject l_newPlayer = Instantiate (c_players[l_randomGO], c_gridPositions [l_randomIndex] + l_verticalOffset, Quaternion.Euler(new Vector3(0,180,0)));
			l_newPlayer.transform.SetParent (c_myBoard);
			s_playerCharacters.Add(l_newPlayer);
			if (!l_mainAssigned) {
				l_newPlayer.GetComponent<PlayerAttack> ().c_myTurn = true;
				l_newPlayer.GetComponent<PlayerAttack> ().c_particleComponent.Play();
				l_mainAssigned = true;
			}
			
			c_gridPositions.RemoveAt (l_randomIndex);
			l_gridPositionsRemoved++;
		}

		int l_enemies = Random.Range (c_enemyChars.c_min, c_enemyChars.c_max);
		for (int l_Count = 0; l_Count < l_enemies; l_Count++) {
			int l_randomIndex = Random.Range (c_gridPositions.Count / 2 - l_gridPositionsRemoved, c_gridPositions.Count);
			int l_randomGO = Random.Range (0, c_enemies.Length);
			GameObject l_newEnemy = Instantiate (c_enemies[l_randomGO], c_gridPositions [l_randomIndex] + l_verticalOffset, Quaternion.identity);
			l_newEnemy.transform.SetParent (c_myBoard);
			s_enemyCharacters.Add(l_newEnemy);
			c_gridPositions.RemoveAt (l_randomIndex);
		}
	}

	void Start(){
		InitialiseList ();
		BoardSetup ();
		BattleSetup ();
	}
}
