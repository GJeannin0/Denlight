using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
	private Rigidbody2D myRigidbody2D;
	private uint shapeIndex = 0;                            // 0 = circle, 1 = circle2, 2 = circle3, 20 = spiky, 21 = spiky2, 22 = spiky3, 40 = triangle, 41 = triangle2, 42 = triangle3,  

	private float movementSpeed;
	[SerializeField] private float baseMovementSpeed = 10.0f;

	private Vector2 right = new Vector2(1.0f, 0.0f);
	private Vector2 upRight = new Vector2(1 / (float)Math.Sqrt(2), 1 / (float)Math.Sqrt(2));
	private Vector2 up = new Vector2(0.0f, 1.0f);
	private Vector2 upLeft = new Vector2(- 1 / (float)Math.Sqrt(2), 1 / (float)Math.Sqrt(2));
	private Vector2 left = new Vector2(- 1.0f, 0.0f);
	private Vector2 downLeft = new Vector2(- 1 / (float)Math.Sqrt(2), - 1 / (float)Math.Sqrt(2));
	private Vector2 down = new Vector2(0.0f, - 1.0f);
	private Vector2 downRight = new Vector2(1 / (float)Math.Sqrt(2), - 1 / (float)Math.Sqrt(2));

	private int life;
	[SerializeField] private int baseLife;

	[SerializeField] private int dropCost;


	[SerializeField] private HoveringLight lightDrop;

	[SerializeField] private Oscillator myOscillator;
	private SpriteRenderer mySpriteRenderer;

	[SerializeField] private float minimunTransparency = 0.50f;
	[SerializeField] private float minimunLight = 0.30f;

	[SerializeField] private Vector3 minimumSize;
	[SerializeField] private Projectile myProjectile;
	[SerializeField] private float projectileCd;
	private float projectileTimer;
	private Camera myCamera;

	void Start()
    {
		myCamera = FindObjectOfType<Camera>();
		myRigidbody2D = GetComponent<Rigidbody2D>();
		movementSpeed = baseMovementSpeed;
		life = baseLife;
		mySpriteRenderer = myOscillator.gameObject.GetComponent<SpriteRenderer>();
    }

   
    void Update()
    {
		if (Input.GetButtonDown("Fire2"))
		{
			if (life > dropCost)
			{
				changeLife(-dropCost);
				Instantiate(lightDrop, transform.position, transform.rotation);
			}
		}

		if (shapeIndex < 40)
		{
			if (shapeIndex < 20)
			{
				if (projectileTimer >= projectileCd)
				{
					if (Input.GetButton("Fire1"))
					{
						Instantiate(myProjectile, transform.position, Quaternion.LookRotation(transform.forward, (new Vector3(myCamera.ScreenToWorldPoint(Input.mousePosition).x, myCamera.ScreenToWorldPoint(Input.mousePosition).y, 0.0f) - transform.position).normalized));
						projectileTimer = 0.0f;
					}
				}
				else
				{
					projectileTimer += Time.deltaTime;
				}
			}

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
	}

	public void changeLife(int amount)
	{
		life += amount;
		float lightPercentage = (float)life /baseLife;
		float remainingLight = minimunLight + lightPercentage * (1.0f - minimunLight);
		mySpriteRenderer.color = new Color(remainingLight, remainingLight, remainingLight, minimunTransparency + lightPercentage * (1.0f - minimunTransparency));
	}
}
