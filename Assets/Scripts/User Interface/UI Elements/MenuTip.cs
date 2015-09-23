using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuTip : MonoBehaviour 
{
	Image tipSprite;
	Text tipText;

	void Awake()
	{
		tipSprite  = this.GetComponent<Image>();
		tipText 	 = this.GetComponentInChildren<Text>();

		tipSprite.enabled = false;
		tipText.enabled = false;
	}
	public void Show()
	{
		tipSprite.enabled = true;
		tipText.enabled = true;
	}

	public void Hide()
	{
		tipSprite.enabled = false;
		tipText.enabled = false;
	}
}
