using UnityEngine;
using System.Collections;

public class ScreenShake : Singleton<ScreenShake> 
{

	Camera camera ; // set this via inspector
	internal  float shake = 0;
	float shakeAmount = 0.2f;
	float decreaseFactor = 1.0f;

	void Start()
	{
		camera = Camera.main; 
	} 

	void Update() 
	{
		if (shake > 0) 
		{
			camera.transform.localPosition = Random.insideUnitSphere * shakeAmount;
			camera.transform.localPosition = new Vector3(camera.transform.localPosition.x,camera.transform.localPosition.y, -15.1f);

			shake -= Time.deltaTime * decreaseFactor;
		} 
		else 
		{
			shake = 0.0f;
			camera.transform.localPosition = new Vector3(0,0,-15.1f);
		}
	}
}
