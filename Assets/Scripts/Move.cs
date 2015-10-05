using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour 
{
	public float speed = 5.0f;
	Vector3 startPos;

	void Start()
	{
		startPos = this.transform.position;
	}
	// Update is called once per frame
	void Update () 
	{
		float newX = transform.position.x + (Time.deltaTime *  speed);
		transform.position = new Vector3(newX , transform.position.y, transform.position.z);

		if(transform.position.x > startPos.x + 28)
		{
			transform.position = startPos;
		}
	}
}
