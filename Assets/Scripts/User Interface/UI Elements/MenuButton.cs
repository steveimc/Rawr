using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuButton : Button
{
	[SerializeField] private GameMode buttonType;
	public GameMode GetButtonType() { return buttonType; }
	
	public void ShowPress()
	{
		AudioManager.instance.Play(Audio.Bank.UI_ACCEPT);
		this.DoStateTransition (Selectable.SelectionState.Pressed, false);
	}
}


