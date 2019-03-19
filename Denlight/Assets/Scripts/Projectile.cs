using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

	private Rigidbody2D myRigidbody2D;

	[SerializeField] private float speed;

    void Start()
    {
		myRigidbody2D = GetComponent<Rigidbody2D>();
		myRigidbody2D.velocity = transform.up * speed;
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == ("Wall"))
		{
			Destroy(gameObject);
		}
	}
}
