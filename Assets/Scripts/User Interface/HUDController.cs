using UnityEngine;
using System.Collections;

public class HUDController : MonoBehaviour 
{
	[SerializeField] private UIPlayer playerLeft; // P1
	[SerializeField] private UIPlayer playerRight; // P2

	int hp = 10;

	void Start()
	{
		playerLeft.Init("Brandt", 0, 10, 10);
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.B))
		{
			playerLeft.HasSword(true);
		}

		if(Input.GetKeyDown(KeyCode.N))
		{
			hp -=1;
			playerLeft.UpdateHP(hp);
		}
	}
}
