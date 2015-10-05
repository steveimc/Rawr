using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum GameMode
{
	SinglePlayer,
	Arcade,
	Local,
	Online,
	Lan,
	None
}

public class Game : MonoBehaviour 
{
	public static Game Instance { get; private set; }

	private void Awake()
	{
		if(Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		Instance = this;
		DontDestroyOnLoad(gameObject);
	}

	private bool isLocalGame = false;
	public bool IsLocalGame
	{
		get
		{
			return isLocalGame;
		}
		set
		{
			DebugLog ("set: " + value.ToString());
			isLocalGame = value;
		}
	}

	private GameMode gameMode = GameMode.None;
	public GameMode GameMode
	{
		get
		{
			return gameMode;
		}
		set
		{
			gameMode = value;
		}
	}

	private string gameScene = "Loading";

	public void StartAsLocalGame()
	{
		IsLocalGame = true;
		gameMode = GameMode.Local;
		StartWithoutNetwork();
	}

	private void StartWithoutNetwork()
	{
		UnityEngine.Networking.NetworkManager nm = FindObjectOfType<UnityEngine.Networking.NetworkManager>();
		nm.dontDestroyOnLoad = false;
		Destroy(nm.gameObject);
		Application.LoadLevel(gameScene);
	}

	public void StartAsSinglePlayer()
	{
		isLocalGame = true;
		gameMode = GameMode.SinglePlayer;
		StartWithoutNetwork();
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
	}

	private void AddButtonListener()
	{
		Button localBtn = GameObject.Find("LocalButton").GetComponent<Button>();
		localBtn.onClick.AddListener( () => StartAsLocalGame() );
	}

	/*
	public bool IsLocalGame()
	{
		Debug.Log("IS LOCAL GAME: " + isLocalGame);
		return isLocalGame;
	}*/

	void OnLevelWasLoaded(int level) 
	{
		if(level == 0)
		{
			AddButtonListener();
		}
	}

	private static bool enableGameDebugLog = false;
	public static void DebugLog(string whatToLog)
	{
		if(enableGameDebugLog)
			Debug.Log(whatToLog);
	}
}
