using UnityEngine;
using System.Collections;

public class HUDController : Singleton<HUDController> 
{
	//[SerializeField] private UIPlayer playerLeft; // P1
	//[SerializeField] private UIPlayer playerRight; // P2

	[SerializeField] private UIPlayer[] players;
	[SerializeField] private StageBanners stageBanner;
	
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
		// Stages go from 0 to 1,  banners go from 1 to 6
		stageBanner.ShowBanner(nextStage + 1);
	}

	public void HeroRespawn(int whichHero)
	{
		players[whichHero-1].RestoreHP();
	}
}
