using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
	private Rigidbody2D myRigidbody2D;
	private uint shapeIndex = 0;							// 0 = circle, 1 = circle2, 2 = circle3, 3 = triangle, 4 = triangle2, 5 = triangle3, 6 = spiky, 7 = spiky2, 8 = spiky3  

	[SerializeField] private float movementSpeed = 10.0f;

	private Vector2 right = new Vector2(1.0f, 0.0f);
	private Vector2 upRight = new Vector2(1 / (float)Math.Sqrt(2), 1 / (float)Math.Sqrt(2));
	private Vector2 up = new Vector2(0.0f, 1.0f);
	private Vector2 upLeft = new Vector2(- 1 / (float)Math.Sqrt(2), 1 / (float)Math.Sqrt(2));
	private Vector2 left = new Vector2(- 1.0f, 0.0f);
	private Vector2 downLeft = new Vector2(- 1 / (float)Math.Sqrt(2), - 1 / (float)Math.Sqrt(2));
	private Vector2 down = new Vector2(0.0f, - 1.0f);
	private Vector2 downRight = new Vector2(1 / (float)Math.Sqrt(2), - 1 / (float)Math.Sqrt(2));


	void Start()
    {
		myRigidbody2D = GetComponent<Rigidbody2D>();
    }

   
    void Update()
    {
		if (Input.GetAxis("Horizontal") > 0.0f)
		{
			if (Input.GetAxis("Vertical") > 0.0f)
			{
				MovePlayer(1);
			}
			else
			{
				if (Input.GetAxis("Vertical") < 0.0f)
				{
					MovePlayer(7);
				}
				else
				{
					MovePlayer(0);
				}
			}
		}
		else
		{
			if (Input.GetAxis("Horizontal") < 0.0f)
			{
				if (Input.GetAxis("Vertical") > 0.0f)
				{
					MovePlayer(3);
				}
				else
				{
					if (Input.GetAxis("Vertical") < 0.0f)
					{
						MovePlayer(5);
					}
					else
					{
						MovePlayer(4);
					}
				}
			}
			else
			{
				if (Input.GetAxis("Vertical") > 0.0f)
				{
					MovePlayer(2);
				}
				else
				{
					if (Input.GetAxis("Vertical") < 0.0f)
					{
						MovePlayer(6);
					}
				}
			}
		}
	}

	private void MovePlayer(uint directionIndex)		// 0 right, 1 up right, 2 up, 3 up left, 4 left, 5 down left, 6 down, 7 down right
	{
		if (directionIndex == 0)
		{
			myRigidbody2D.velocity = right * movementSpeed;
		}
		if (directionIndex == 1)
		{
			myRigidbody2D.velocity = upRight * movementSpeed;
		}
		if (directionIndex == 2)
		{
			myRigidbody2D.velocity = up * movementSpeed;
		}
		if (directionIndex == 3)
		{
			myRigidbody2D.velocity = upLeft * movementSpeed;
		}
		if (directionIndex == 4)
		{
			myRigidbody2D.velocity = left * movementSpeed;
		}
		if (directionIndex == 5)
		{
			myRigidbody2D.velocity = downLeft * movementSpeed;
		}
		if (directionIndex == 6)
		{
			myRigidbody2D.velocity = down * movementSpeed;
		}
		if (directionIndex == 7)
		{
			myRigidbody2D.velocity = downRight * movementSpeed;
		}
		// mySmallMovement.MoveOrigin(transform.position);
	}
}
