using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

	public bool displayGridGizmos = true;
	public float CharOffset;
	public Node[,] grid;
	public Vector3 gridWorldSize;
	public float nodeRadius;
	public LayerMask unwalkableMask;
	public TerrainType[] walkableRegions;

	float nodeDiameter;
	int gridSizeX;
	int gridSizeZ;


	// Use this for initialization
	void Awake () {
		nodeDiameter = nodeRadius * 2;
		gridSizeX = (int)(gridWorldSize.x / nodeDiameter);
		gridSizeZ = (int)(gridWorldSize.z / nodeDiameter);
		CreateGrid ();
	}

	public int MaxSize{
		get{
			return gridSizeX * gridSizeZ;
		}
	}

	void CreateGrid(){
		grid = new Node[gridSizeX, gridSizeZ];
		Vector2 bob = new Vector2 (transform.position.x, transform.position.z);
		Vector2 worldBottomLeft = bob - (Vector2.right * (gridWorldSize.x / 2)) - (Vector2.up * (gridWorldSize.z / 2));

		for (int x = 0; x < gridSizeX; x++) {
			for (int z = 0; z < gridSizeZ; z++) {
				Vector2 worldPoint = worldBottomLeft + (Vector2.right * (x * nodeDiameter + nodeRadius)) + (Vector2.up * (z * nodeDiameter + nodeRadius));
				bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask));

				int movementPenalty = 0;

				//

				grid [x, z] = new Node (walkable, worldPoint, x, z, movementPenalty);
			}
		}
	}

	public Node NodeFromWorld(Vector3 wPos){
		float percentX = (wPos.x + gridWorldSize.x / 2) / gridWorldSize.x;
		float percentZ = (wPos.z + gridWorldSize.z / 2) / gridWorldSize.z;
		percentX = Mathf.Clamp01 (percentX);
		percentZ = Mathf.Clamp01 (percentZ);

		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int z = Mathf.RoundToInt((gridSizeZ - 1) * percentZ);
		return grid[x, z];
	}


	void OnDrawGizmos(){
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 0, gridWorldSize.z));
		if (grid != null && displayGridGizmos) {
			foreach (Node n in grid) {
				Gizmos.color = (n.walkable) ? Color.white : Color.red;
				Gizmos.DrawCube (n.nodePosition, Vector3.one * (nodeDiameter - 0.1f));
			}
		}
	}

	public List<Node> getNeighbours(Node current){
		List<Node> neighbours = new List<Node>();

		for(int x = -1; x <= 1; x++){
			for(int z = -1; z <= 1; z++){
				if(x == 0 && z == 0){
					continue;
				}
				else{
					int checkX = current.gridX + x;
					int checkZ = current.gridZ + z;

					if(checkX >= 0 && checkX < gridSizeX && checkZ >= 0 && checkZ < gridSizeZ){
						neighbours.Add(grid[checkX, checkZ]);
					}
				}
			}
		}
		return neighbours;
	}

	[System.Serializable]
	public class TerrainType{
		public LayerMask TerrainMask;
		public int TerrainPenalty;
	}

}
