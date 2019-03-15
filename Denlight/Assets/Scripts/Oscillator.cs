using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
	[SerializeField] private float minimumSpeedX;
	[SerializeField] private float maximumSpeedX;
	[SerializeField] private float speedVariationX;
	[SerializeField] private float rangeX;
	private float speedX;

	[SerializeField] private float minimumSpeedY;
	[SerializeField] private float maximumSpeedY;
	[SerializeField] private float speedVariationY;
	[SerializeField] private float rangeY;
	private float speedY;

	void Start()
    {
		speedX = Random.Range(minimumSpeedX, maximumSpeedX);
		speedY = Random.Range(minimumSpeedY, maximumSpeedY);
	}


    void Update()
    {
		updateSpeed();
		transform.localPosition = new Vector3(Mathf.Cos(Time.time * speedX) * rangeX, Mathf.Cos(Time.time * speedY) * rangeY, 0.0f);
		Debug.Log(Mathf.Cos(Time.time * speedX));
	}

	private void updateSpeed()
	{
		speedX += Random.Range(-1.0f, 1.0f) * speedVariationX * Time.deltaTime;
		speedY += Random.Range(-1.0f, 1.0f) * speedVariationY * Time.deltaTime;

		if (speedX < minimumSpeedX)
		{
			speedX = minimumSpeedX;
		}
		if (speedX > maximumSpeedX)
		{
			speedX = maximumSpeedX;
		}
		if (speedY < minimumSpeedY)
		{
			speedY = minimumSpeedY;
		}
		if (speedY > maximumSpeedY)
		{
			speedY = maximumSpeedY;
		}
	}
}
