using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] private int baseLife;
	private int life;

	private Vector3 baseScale;
	[SerializeField] private Vector3 minimumScale;

	[SerializeField] private float minimunTransparency = 0.50f;
	[SerializeField] private float minimunLight = 0.30f;
	private SpriteRenderer mySpriteRenderer;

	private Rigidbody2D myRigidbody2D;

	private Player myPlayer;

	[SerializeField] private float knockbackSensibility;
	[SerializeField] private GameObject mySpriteHolder;

    void Start()
    {
		myRigidbody2D = GetComponent<Rigidbody2D>();
		baseScale = transform.localScale;
		myPlayer = FindObjectOfType<Player>();
		life = baseLife;
		mySpriteRenderer = mySpriteHolder.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == ("Attack"))
		{
			ChangeLife(-myPlayer.GetAttackDamage());
			Knockback(myPlayer.GetAttackKnockback(), collision.gameObject.transform.position);
			rotateTowards(myPlayer.transform.position);
			Destroy(collision.gameObject);
		}
	}

	private void Knockback(float force, Vector3 knockerPos)
	{
		Vector2 direction = new Vector2(transform.position.x - knockerPos.x, transform.position.y - knockerPos.y).normalized;
		myRigidbody2D.velocity += force * direction * knockbackSensibility;
	}

	public void ChangeLife(int amount)
	{
		life += amount;
		if (life <= 0)
		{
			Destroy(gameObject);
		}
		float lightPercentage = (float)life / baseLife;
		float remainingLight = minimunLight + lightPercentage * (1.0f - minimunLight);
		transform.localScale = (baseScale - minimumScale) * lightPercentage + minimumScale;
		mySpriteRenderer.color = new Color(mySpriteRenderer.color.r, mySpriteRenderer.color.g, mySpriteRenderer.color.b, minimunTransparency + lightPercentage * (1.0f - minimunTransparency));
	}

	private void rotateTowards(Vector3 target)
	{
		transform.up = (target - transform.position).normalized;
	}
}
