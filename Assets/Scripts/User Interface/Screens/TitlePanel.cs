using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitlePanel : BaseScreen 
{
	[SerializeField] private ControllerButton playButton;

	void Start()
	{
		playButton.ShowHighlighted();
	}

	public void OnPlayButton()
	{
		ScreenManager.instance.Push<ModePanel>();
	}

	protected override void OnControllerInput(ControllerEvent controllerInput)
	{
		switch(controllerInput)
		{
		case ControllerEvent.A_Button:
			playButton.ShowPress();
			OnPlayButton();
			break;
		}
	}
}
