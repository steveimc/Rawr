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
		this.DoStateTransition (Selectable.SelectionState.Pressed, false);
	}

	public void ShowHighlighted()
	{
		this.DoStateTransition (Selectable.SelectionState.Highlighted, false);
	}

	public void ShowNormal()
	{
		this.DoStateTransition (Selectable.SelectionState.Normal, false);
	}

	public void ShowDisabled()
	{
		this.DoStateTransition (Selectable.SelectionState.Disabled, false);
	}

	public void DoClick()
	{
		this.OnPointerClick(null);
	}
}
