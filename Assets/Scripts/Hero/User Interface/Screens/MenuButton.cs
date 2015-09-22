using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum MenuButtons 
{
	SinglePlayer,
	Arcade,
	Local,
	Online,
	Lan
}

public class MenuButton : Button
{
	[SerializeField] private MenuButtons buttonType;
	public MenuButtons GetButtonType() { return buttonType; }
	
	public void ShowPress()
	{
		this.DoStateTransition (Selectable.SelectionState.Pressed, false);
	}
}


