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

	private Player myPlayer;

    // Start is called before the first frame update
    void Start()
    {
		baseScale = transform.localScale;
		myPlayer = FindObjectOfType<Player>();
		life = baseLife;
		mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == ("Attack"))
		{
			changeLife(-myPlayer.GetAttackDamage());
			Destroy(collision.gameObject);
		}
	}

	public void changeLife(int amount)
	{
		life += amount;
		if (life <= 0)
		{
			Destroy(gameObject);
		}
		float lightPercentage = (float)life / baseLife;
		float remainingLight = minimunLight + lightPercentage * (1.0f - minimunLight);
		transform.localScale = (baseScale - minimumScale) * lightPercentage + minimumScale;
		mySpriteRenderer.color = new Color(mySpriteRenderer.color.a, mySpriteRenderer.color.b, mySpriteRenderer.color.g, minimunTransparency + lightPercentage * (1.0f - minimunTransparency));
	}
}
