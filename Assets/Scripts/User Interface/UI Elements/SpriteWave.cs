using UnityEngine;
using System.Collections;

public class SpriteWave : MonoBehaviour {

	public bool goLeft;
	public float MoveSpeed = 5.0f;
	
	public float frequency = 20.0f;  // Speed of sine movement
	public float magnitude = 0.1f;   // Size of sine movement
	private Vector3 axis;
	
	private Vector3 pos;
	
	void Start () {
		pos = transform.position;
		axis = transform.right;  // May or may not be the axis you want		

		if(goLeft)
			axis = new Vector3 (-1.0f, 0.0f, 0.0f);
	}
	
	void Update () {
		pos += transform.up * Time.deltaTime * MoveSpeed;
		Vector3 newPos = pos + axis * Mathf.Sin (Time.time * frequency) * magnitude;
		newPos.z = transform.position.z;
		transform.position = newPos;
	}
}
