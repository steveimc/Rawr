using UnityEngine;
using System.Collections;
using System;

public class KeyboardPopup : BaseScreen 
{
	public void Init(Action<string> keyboardCallback)
	{
		GetComponentInChildren<ScreenKeyboard>().Init(keyboardCallback);
	}

	protected override void OnControllerInput(ControllerEvent controllerInput)
	{
	}
}