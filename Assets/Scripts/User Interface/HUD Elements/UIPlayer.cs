using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour 
{
	[SerializeField] Image imgHeroIcon;
	[SerializeField] Text txtUsername;
	[SerializeField] UIToggleImage imgWeapon;

	[SerializeField] UIHealthPoints uiHealthPoints;
	//[SerializeField] UIFilledBar barHealth;
	[SerializeField] UIFilledBar barFatigue;

	[SerializeField] Sprite[] heroIcons;
	
	public void Init( string playerName, int whichHero, int maxHealth, int maxFatigue)
	{
		//txtUsername.text = playerName;
		if(whichHero == 1)
			txtUsername.text = "Embla";
		else
			txtUsername.text = "Brandt";

		//barHealth.Init(maxHealth, maxHealth);
		barFatigue.Init(maxFatigue, maxFatigue);

		if(whichHero == 1)
			imgHeroIcon.sprite = heroIcons[0];
		else
			imgHeroIcon.sprite = heroIcons[1];
	}

	public void HasSword(bool hasSword)
	{
		imgWeapon.Toggle(hasSword);
	}

	public void UpdateFatigue(int fatigue)
	{
		barFatigue.UpdateValue(fatigue);
	}

	public void UpdateHP(int hp)
	{
		uiHealthPoints.UpdateLife(hp);
	}

	public void RestoreHP()
	{
		uiHealthPoints.RestoreFullLife();
	}
}
