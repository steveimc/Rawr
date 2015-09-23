using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour 
{
	[SerializeField] Image imgHeroIcon;
	[SerializeField] Text txtUsername;
	[SerializeField] Image imgWeapon;

	[SerializeField] UIFilledBar barHealth;
	[SerializeField] UIFilledBar barFatigue;
	
	public void Init( string playerName, int whichHero, int maxHealth, int maxFatigue)
	{
		// set img
		txtUsername.text = playerName;
		barHealth.Init(maxHealth, maxHealth);
		barFatigue.Init(maxFatigue, 0);
	}

	public void HasSword(bool hasSword)
	{
		if(hasSword)
			imgWeapon.color = Color.red;
		else
			imgWeapon.color = Color.grey;
	}

	public void UpdateHP(int hp)
	{
		barHealth.UpdateValue(hp);
	}
}
