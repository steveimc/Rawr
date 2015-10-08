using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControllerButton : Button 
{
	private Text buttonText;

	protected override void Awake()
	{
		base.Awake();
		buttonText = GetComponentInChildren<Text>();
	}

	public string GetText()
	{
		return buttonText.text;
	}

	public void SetText( string newText)
	{
		buttonText.text = newText;
	}

	public void ShowPress()
	{
		AudioManager.instance.Play(Audio.Bank.UI_ACCEPT);
		this.DoStateTransition (Selectable.SelectionState.Pressed, false);
	}

	public void ShowHighlighted()
	{
		AudioManager.instance.Play(Audio.Bank.UI_HOVER);
		this.DoStateTransition (Selectable.SelectionState.Highlighted, false);
	}

	public void ShowNormal()
	{
		this.DoStateTransition (Selectable.SelectionState.Normal, false);
	}

	public void ShowDisabled()
	{
		AudioManager.instance.Play(Audio.Bank.UI_BACK);
		this.DoStateTransition (Selectable.SelectionState.Disabled, false);
	}

	public void DoClick()
	{
		this.OnPointerClick(null);
	}
}
