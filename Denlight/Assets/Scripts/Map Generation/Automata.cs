using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Automata : MonoBehaviour
{
	public struct Cell
	{
		public bool alive;
		public bool willLive;
		public int room;
	}

	[SerializeField] private GameObject aliveCell;
	[SerializeField] private GameObject deadCell;

	[SerializeField] private int seed;
	[SerializeField] private float amountOfLiving;
	[SerializeField] private int height;
	[SerializeField] private int length;

	private float donut = 5;
	private Cell[,] map;


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
				map[i, j] = new Cell();
				if (Random.value >= amountOfLiving)
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


		DrawMap();
		
	}

	private void DrawMap()
	{
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < length; j++)
			{
				if (map[i, j].alive)
				{
					Instantiate(aliveCell, new Vector3(j, i, 0.0f), transform.rotation);
				}
				else
				{
					Instantiate(deadCell, new Vector3(j, i, 0.0f), transform.rotation);
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

					if (map[j + b.x, i + b.y].alive)
					{
						neighbours++;
					}
				}

				if (map[j, i].alive && (neighbours >= neighboursToLive && neighbours < neighboursToDie))
				{
					map[j, i].willLive = true;
				}
				else if (!map[j, i].alive && neighbours >= neighboursToSpawn)
				{
					map[j, i].willLive = true;
				}
				else
				{
					map[j, i].willLive = false;
				}

			}
		}

		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < length; j++)
			{
				map[j, i].alive = map[j, i].willLive;
			}
		}
	}
}
