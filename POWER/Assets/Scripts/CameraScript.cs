using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript: MonoBehaviour {

	public Transform target;

    [Range(0,1)]
	public float smoothSpeed;
	public Vector3 offset;
	Vector3 smoothedPosition;

	private float shakeTimeRemaining, shakePower, shakeFadeTime;

	void FixedUpdate()
	{
		Vector3 desiredPosition = target.position + offset;
		smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
		transform.position = smoothedPosition;

		//transform.LookAt(target);
	}
	void LateUpdate()
	{
		if (shakeTimeRemaining > 0)
		{
			shakeTimeRemaining -= Time.deltaTime;

			float x = Random.Range(-1f,1f) * shakePower;
			float y = Random.Range(-1f,1f) * shakePower;

			transform.localPosition += new Vector3(x, y, 0);

			shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime);
		}
	}

	public void StartShake(float length, float power)
	{
		shakeTimeRemaining = length;
		shakePower = power;
		shakeFadeTime = power/length;
	}
}