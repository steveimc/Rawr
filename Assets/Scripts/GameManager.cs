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

	internal 	HeroStatus[] player = new HeroStatus[2];
	public 		GameObject m_FrostNovaCopy;

	private 	Stage m_Stage;	
	private 	EnemySpawner m_EnemySpawner;

	private 	int playerCount = 0;
	private 	int m_iCurrentEssences = 0;
	internal	int m_iEnemiesOnScreen = 0;
	private	 	int m_iNumOfStages = 5;

	private 	bool m_dieOnce;

	public static GameManager Instance { get; private set; }
	
	private void Awake()
	{
		m_dieOnce = false;

		if(Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		Instance = this;
		//DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		if(Game.Instance.IsLocalGame)
		{
			InstantiatePlayers();
		
			m_Stage = new Stage(OnStageCompleted);
			m_Stage.Init(0);

			m_EnemySpawner = GetComponent<EnemySpawner>();
			m_EnemySpawner.InitSpawner(m_Stage.GetCurrentStage(), MaxEnemiesOnScreen(m_Stage.GetCurrentStage()), m_Stage.GetEnemyTypes());
		}
	}

	private void OnStageCompleted(int stage)
	{
		m_iCurrentEssences = 0;
		HUDController.instance.UpdateCurrentEssences(m_iCurrentEssences);

		if(stage + 1 < m_iNumOfStages)
		{
			m_Stage.Init(stage + 1);
			m_EnemySpawner.InitSpawner(m_Stage.GetCurrentStage(), MaxEnemiesOnScreen(m_Stage.GetCurrentStage()), m_Stage.GetEnemyTypes());
		}
		{
			// TODO: Else -> Boss fight
		}
		for(int i = 0; i < player.Length; i++)
		{
			if(player[i] != null)
			{
				player[i].RestoreHeroHealth();
			}
		}
	}

	public void OnEssenceCaptured()
	{
		m_iCurrentEssences++;
		m_Stage.UpdateEssences(m_iCurrentEssences);
		HUDController.instance.UpdateCurrentEssences(m_iCurrentEssences);
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
	
/*	void OnLevelWasLoaded(int level) 
	{
		if(level == 0)
		{
			Instance = null;
			Destroy(this.gameObject);
		}
	}*/

	private int MaxEnemiesOnScreen(int stage)
	{
		int maxEnemies =  (stage +1) * 2;
		return maxEnemies;
	}

	public void HeroDied()
	{
		int deadPlayers = 0;
		foreach(HeroStatus hero in player)
		{
			if(hero != null && hero.GetHealth() <= 0)
				deadPlayers++;
		}

		if((deadPlayers > 0))
		{
			if(player[1] == null && !m_dieOnce)
			{
				HUDController.instance.OnGameOver();
				Invoke("RestartGame", 3.0f);
				m_dieOnce = true;
			}
			else if(deadPlayers > player.Length && !m_dieOnce)
			{
				HUDController.instance.OnGameOver();
				Invoke("RestartGame", 3.0f);
				m_dieOnce = true;
			}
		}	
	}

	private void RestartGame()
	{
		Application.LoadLevel("Loading");
	}

}