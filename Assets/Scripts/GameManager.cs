using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public enum HeroId
	{
		NONE,
		PLAYER1,
		PLAYER2
	}

	private HeroId id;

	public static GameManager Instance { get; private set; }
	
	private void Awake()
	{
		if(Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}

		id = HeroId.NONE;
		Instance = this;
		DontDestroyOnLoad(gameObject);
	}

	public int AssignPlayerId(string sPlayerName = "")
	{
		switch(sPlayerName)
		{
			case "Player1":
				id = HeroId.PLAYER1;
				return (int)id;

			case "Player2":
				id = HeroId.PLAYER2;
				return (int)id;

			default:
				if(id == HeroId.NONE)
				{
					id = HeroId.PLAYER1;
					return (int)id;
				}
				else
				{
					id = HeroId.PLAYER2;
					return (int)id;
				}

		}

	}
}
