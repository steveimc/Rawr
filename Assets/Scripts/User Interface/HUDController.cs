using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDController : Singleton<HUDController> 
{
	//[SerializeField] private UIPlayer playerLeft; // P1
	//[SerializeField] private UIPlayer playerRight; // P2

	[SerializeField] private UIPlayer[] players;
	[SerializeField] private StageBanners stageBanner;
	[SerializeField] private GameObject gameOverBanner;

	[SerializeField] private Text stageText;
	[SerializeField] private Text currentEssences;
	[SerializeField] private Text totalEssences;
	
	void Start()
	{
		//playerLeft.Init("Brandt", 0, 10, 10);
	}

	public void InitUIPlayer(string playerName, int whichHero, int maxHealth, int maxFatigue)
	{
		players[whichHero-1].Init(playerName, whichHero, maxHealth, maxFatigue);
	}

	public void UpdateHasSword(int heroID, bool state)
	{
		players[heroID-1].HasSword(state);
	}

	public void UpdateHeroHp(int heroID, int newHP)
	{
		players[heroID-1].UpdateHP(newHP);
	}

	public void UpdateHeroEnergy(int heroID, int newEnergy)
	{
		players[heroID-1].UpdateFatigue(newEnergy);
	}

	public void ShowStageBanner(int nextStage)
	{
		for(int i = 0; i < players.Length; i++)
			players[i].RestoreHP();

		SetStageText(nextStage+1);

		// Stages go from 0 to 1,  banners go from 1 to 6
		if(stageBanner != null)
			stageBanner.ShowBanner(nextStage + 1);
	}

	public void SetTotalEssences(int essencesToPass)
	{
		currentEssences.text = "0";
		totalEssences.text = essencesToPass.ToString();
	}

	public void UpdateCurrentEssences(int currEssences)
	{
		currentEssences.text = currEssences.ToString();
	}

	private void SetStageText(int stage)
	{
		if(stage < 6)
			stageText.text = "Stage " + stage;
		else
			stageText.text = "Final Boss";
	}

	public void OnGameOver()
	{
		AudioManager.instance.PlayWorld(Audio.Bank.GAME_OVER);
		gameOverBanner.SetActive(true);
	}
}
