using UnityEngine;
using System.Collections;

public class HUDController : Singleton<HUDController> 
{
	[SerializeField] private UIPlayer playerLeft; // P1
	[SerializeField] private UIPlayer playerRight; // P2

	[SerializeField] private StageBanners stageBanner;

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

	public void InitUIPlayer(string playerName, int whichHero, int maxHealth, int maxFatigue)
	{
		if(whichHero == 1)
			playerLeft.Init(playerName, whichHero, maxHealth, maxFatigue);
		else
			playerRight.Init(playerName, whichHero, maxHealth, maxFatigue);
	}

	public void UpdateHeroHp(int heroID, int newHP)
	{
		if(heroID == 1)
			playerLeft.UpdateHP(newHP);
		else
			playerRight.UpdateHP(newHP);
	}

	public void ShowStageBanner(int nextStage)
	{
		// Stages go from 0 to 1,  banners go from 1 to 6
		stageBanner.ShowBanner(nextStage + 1);
	}
}
