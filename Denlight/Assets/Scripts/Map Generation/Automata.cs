using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Node
{
	public List<Node> neighbors;
	public Vector2 pos;

	public bool isFree;

	public bool hasBeenVisited = false;
	public bool isPath = false;

	public Node cameFrom = null;

	public float cost;
	public float currentCost = -1;
}

public class Automata : MonoBehaviour
{
	public struct Cell
	{
		public bool alive;
		public bool willLive;
		public uint room;
		public uint typeIndex;          // 1 wall, 2 void, 3 walkable

		public Vector2 pos;
	}

	public struct Room
	{
		public int index;
		public int typeIndex;				// 1 start, 2 End, 3 void, 4 freeloot, 5 danger              Reload une map si trop peu de walkable ou salles trop proches
		public List<int> directNeigbors;														// Set a start based on amount of neigbors + random, set a end far enough from start, if cul de sac = > freeloot et spread void/danger
	}

	private float infiniteCheck = 0;
	[SerializeField] private GameObject aliveCell;
	[SerializeField] private GameObject roomCell;
	[SerializeField] private GameObject room2Cell;
	[SerializeField] private GameObject room3Cell;
	[SerializeField] private GameObject room4Cell;
	[SerializeField] private GameObject room5Cell;
	[SerializeField] private GameObject room6Cell;
	[SerializeField] private GameObject room7Cell;
	[SerializeField] private GameObject room8Cell;


	[SerializeField] private GameObject deadCell;
	[SerializeField] private GameObject wallCell;

	[SerializeField] private int seed;
	[SerializeField] private float amountOfLiving;
	[SerializeField] private int height;
	[SerializeField] private int length;

	private Cell[,] map;
	private Cell[,] reRoomingMap;
	private Room[] rooms;
	private uint currentRoomIndex = 1;

	private float minimumRange = 0.00001f;

	[SerializeField] private int neighboursToSpawn;
	[SerializeField] private int neighboursToLive;
	[SerializeField] private int neighboursToDie;

	void Start()
	{
		map = new Cell[height, length];
		Random.InitState(seed);
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < length; j++)
			{
				map[i, j].room = 0;
				map[i, j] = new Cell();
				map[i, j].pos = new Vector2(i, j);
				if (Random.value <= amountOfLiving)
				{
					map[i, j].alive = true;
				}
				else
				{
					map[i, j].alive = false;
				}
			}
		}

		Step();
		Step();
		Step();
		Step();
		Step();
		Step();
		Step();
		Step();
		Step();
		Step();
		Step();
		Step();
		Step();
		Step();

		FindRooms(map);
		DigTunnels(map);

		if (rooms.Length > 1)
		{
			ConnectEveryRoom(map);
		}

		DrawRooms(map);
		//DrawMap(map);

	}

	private void DrawMap(Cell[,] map)
	{
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < length; j++)
			{
				if (map[i, j].room == 1)
				{
					Instantiate(roomCell, new Vector3(j, i, 0.0f), transform.rotation);
				}
				else
				{
					if (map[i, j].room == 2)
					{
						Instantiate(room2Cell, new Vector3(j, i, 0.0f), transform.rotation);
					}
					else

					if (!map[i, j].alive)
					{
						Instantiate(deadCell, new Vector3(j, i, 0.0f), transform.rotation);
					}
				}
			}
		}
	}

	private void Step()
	{
		BoundsInt bounds = new BoundsInt(-1, -1, 0, 3, 3, 1);

		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < length; j++)
			{
				int neighbours = 0;

				foreach (Vector2Int b in bounds.allPositionsWithin)
				{
					if (b.x == 0 && b.y == 0) continue;
					if (j + b.x < 0 || j + b.x >= length || i + b.y < 0 || i + b.y >= height) continue;

					if (map[i + b.y, j + b.x].alive)
					{
						neighbours++;
					}
				}

				if (map[i, j].alive && (neighbours >= neighboursToLive && neighbours < neighboursToDie))
				{
					map[i, j].willLive = true;
				}
				else if (!map[i, j].alive && neighbours >= neighboursToSpawn)
				{
					map[i, j].willLive = true;
				}
				else
				{
					map[i, j].willLive = false;
				}

			}
		}

		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < length; j++)
			{
				map[i, j].alive = map[i, j].willLive;
			}
		}
	}

	private void FindRooms(Cell[,] map)
	{
		
		currentRoomIndex = 0;
		BoundsInt bounds = new BoundsInt(-1, -1, 0, 3, 3, 1);

		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < length; j++)
			{
				

				if (!map[i, j].alive) continue;
				if (map[i, j].room != 0) continue;

				List<Vector2Int> openList = new List<Vector2Int>();
				List<Vector2Int> closedList = new List<Vector2Int>();

				openList.Add(new Vector2Int(i, j));

				
				currentRoomIndex++;

				while (openList.Count > 0)
				{
					map[openList[0].x, openList[0].y].room = currentRoomIndex;
					closedList.Add(openList[0]);

					foreach (Vector2Int b in bounds.allPositionsWithin)
					{
						if (b.x == 0 && b.y == 0) continue;

						if (b.x != 0 && b.y != 0) continue;

						Vector2Int pos = new Vector2Int(openList[0].x + b.x, openList[0].y + b.y);

						if (pos.x < 0 || pos.x >= length || pos.y < 0 || pos.y >= height) continue;

						if (!map[pos.x, pos.y].alive)
						{
							map[pos.x, pos.y].typeIndex = 1;
							map[pos.x, pos.y].room = currentRoomIndex;
							continue;
						}

						if (map[pos.x, pos.y].room != 0) continue;

						if (closedList.Contains(pos)) continue;

						if (openList.Contains(pos)) continue;

						openList.Add(new Vector2Int(pos.x, pos.y));

					}
					openList.RemoveAt(0);
					
				}
				

			}
		}
		rooms = new Room[currentRoomIndex];

		for (int i = 0; i < currentRoomIndex; i++)
		{
			rooms[i].index = i+1;
			rooms[i].directNeigbors = new List<int>();
		}
	}

	private void DigTunnels(Cell[,] map)
	{
		List<Cell>[] walls = new List<Cell>[currentRoomIndex];

		for (int i = 0; i < currentRoomIndex; i++)
		{
			walls[i] = new List<Cell>();
		}

			for (int i = 0; i < currentRoomIndex; i++)
		{
			for (int j = 0; j < height; j++)
			{
				for (int k = 0; k < length; k++)
				{
					if (map[j, k].typeIndex == 1 && map[j, k].room == i+1)
					{
						walls[i].Add(map[j, k]);
					}
				}
			}
		}

		for (int i = 0; i < currentRoomIndex; i++)
		{


			Cell[] closestWalls = new Cell[2];

			float minimumDistance = height + length;
			int connectedRoomIndex = 0;

			for (int j = 0; j < currentRoomIndex; j++)
			{
				// Digs the smalest tunnel to connect room i to room j


				if (j == i) continue;

				if (rooms[j].directNeigbors.Count > 0)
				{
					bool alreadyConnected = false;
					for (int l = 0; l < rooms[j].directNeigbors.Count; l++)    // if room j connected to room i Continue
					{
						if (rooms[j].directNeigbors[l] == i + 1)
						{
							alreadyConnected = true;
							break;
						}
					}
					if (alreadyConnected) continue;
				}

				for (int k = 0; k < walls[i].Count; k++)
				{
					for (int m = 0; m < walls[j].Count; m++)
					{
						if (minimumDistance > (walls[i][k].pos - walls[j][m].pos).magnitude)
						{
							minimumDistance = (walls[i][k].pos - walls[j][m].pos).magnitude;
							closestWalls[0] = walls[i][k];
							closestWalls[1] = walls[j][m];
							connectedRoomIndex = j+1;
						}
					}
				}


				

				

				// Add walls around the tunnel

				// Digs the smalest tunnel to connect room i to room j
			}
	

			if (connectedRoomIndex != 0)
			{
				DigFromTo(map,closestWalls[0].pos, closestWalls[1].pos);

				rooms[i].directNeigbors.Add(connectedRoomIndex);
				rooms[connectedRoomIndex - 1].directNeigbors.Add(i + 1);
			}

			// Dig tunnel from Walls[0] to Walls[1] and set these rooms as connected
		}
	}

	private void DigFromTo(Cell[,] map,Vector2 from, Vector2 to)
	{
		Vector2 direction = to - from;
		int xSign = 0;
		int ySign = 0;

		if (direction.x >= 0)
		{
			xSign = 1;
		}
		else
		{
			xSign = -1;
		}

		if (direction.y >= 0)
		{
			ySign = 1;
		}
		else
		{
			ySign = -1;
		}

		direction = new Vector2(Mathf.Abs(direction.x), Mathf.Abs(direction.y));
		Vector2 lastCell = from;
		map[(int)lastCell.x, (int)lastCell.y].alive = true;

		while(direction.x + direction.y > 0)
		{
			if (Random.Range(minimumRange, 1) * direction.x >= Random.Range(minimumRange, 1) * direction.y)
			{
				lastCell += new Vector2(xSign, 0);
				direction.x--;
			}
			else
			{
				lastCell += new Vector2(0, ySign);
				direction.y--;
			}
			map[(int)lastCell.x, (int)lastCell.y].alive = true;
		}
	}

	private void ConnectEveryRoom(Cell[,] mapToReroom)
	{
		
		infiniteCheck++;

		if (infiniteCheck >= 10)
		{
			map = reRoomingMap;
			Debug.Log("Exit by infinite");
			return;
		}

		reRoomingMap = mapToReroom;

		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < length; j++)
			{
				reRoomingMap[i, j].room = 0;
				if (reRoomingMap[i, j].alive)
				{
					reRoomingMap[i, j].typeIndex = 3;
				}
				else
				{
					reRoomingMap[i, j].typeIndex = 2;
				}
			}
		}
		FindRooms(reRoomingMap);

		if (rooms.Length > 1)
		{
			DigTunnels(reRoomingMap);
			ConnectEveryRoom(reRoomingMap);
		}
		else
		{
			map = reRoomingMap;
		}

		map = reRoomingMap;
	}

	private void DrawRooms(Cell[,] map)
	{
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < length; j++)
			{
				if (map[i, j].typeIndex == 1)
				{
					Instantiate(room7Cell, new Vector3(j, i, 0.0f), transform.rotation);
				}
				else
					if (map[i, j].alive)
				{
					if (map[i, j].room == 1)
					{
						Instantiate(roomCell, new Vector3(j, i, 0.0f), transform.rotation);
					}
					else
					{
						if (map[i, j].room == 2)
						{
							Instantiate(room2Cell, new Vector3(j, i, 0.0f), transform.rotation);
						}
						else
						{
							if (map[i, j].room == 3)
							{
								Instantiate(room3Cell, new Vector3(j, i, 0.0f), transform.rotation);
							}
						}
					}
				}
				else
				{
					if (map[i, j].room == 1)
					{
						Instantiate(room4Cell, new Vector3(j, i, 0.0f), transform.rotation);
					}
					else
					{
						if (map[i, j].room == 2)
						{
							Instantiate(room5Cell, new Vector3(j, i, 0.0f), transform.rotation);
						}
						else
						{
							if (map[i, j].room == 3)
							{
								Instantiate(room6Cell, new Vector3(j, i, 0.0f), transform.rotation);
							}
							else
							{
								Instantiate(deadCell, new Vector3(j, i, 0.0f), transform.rotation);
							}
						}
					}
				}

			
		
				
			}
		}
	}
}
