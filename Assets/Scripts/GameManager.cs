using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	[SerializeField] GameObject spawnPoint;
	[SerializeField] GameObject playerPrefab;

	private int playerCount = 0;

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

	private void Start()
	{
		if(Game.Instance.IsLocalGame)
		{
			InstantiatePlayers();
		}
	}

	private void InstantiatePlayers()
	{
		Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
		// If its 2Players Local spawn second player
		if(Game.Instance.GameMode == GameMode.Local)
			Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
	}

	public int PlayerJoined()
	{
		playerCount ++;
		return playerCount;
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

	void OnLevelWasLoaded(int level) 
	{
		if(level == 0)
		{
			Instance = null;
			Destroy(this.gameObject);
		}
	}
}
