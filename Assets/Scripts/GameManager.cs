using UnityEngine;
using System.Collections;

public enum HeroId
{
	NONE,
	PLAYER1,
	PLAYER2
}

public class GameManager : MonoBehaviour 
{
	[SerializeField] GameObject spawnPoint;
	[SerializeField] GameObject playerPrefab;

	internal HeroStatus[] player = new HeroStatus[2];
	public GameObject m_FrostNovaCopy;
	private Stage m_Stage;	
	private int playerCount = 0;
	private int m_iCurrentEssences = 0;
	internal int m_iEnemiesOnScreen = 0;

	public static GameManager Instance { get; private set; }
	
	private void Awake()
	{
		if(Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		Instance = this;
		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		if(Game.Instance.IsLocalGame)
		{
			InstantiatePlayers();
			m_Stage = new Stage(OnStageCompleted);
			m_Stage.Init(0);
		}
	}
	
	private void OnStageCompleted(int stage)
	{
		m_Stage.Init(stage++);
	}

	public void OnEssenceCaptured()
	{
		m_iCurrentEssences++;
		m_Stage.UpdateEssences(m_iCurrentEssences);
	}

	private void InstantiatePlayers()
	{
		Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
		// If its 2Players Local spawn second player
		if(Game.Instance.GameMode == GameMode.Local)
			Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
	}

	public int PlayerJoined(HeroStatus hero)
	{
		player[playerCount] = hero;
			
		playerCount ++;
		return playerCount;
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