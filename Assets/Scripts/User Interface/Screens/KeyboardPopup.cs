using UnityEngine;
using System.Collections;
using System;

public class KeyboardPopup : BaseScreen 
{
	public void Init(String hint, Action<string> keyboardCallback)
	{
		GetComponentInChildren<ScreenKeyboard>().Init(hint, keyboardCallback);
	}

	protected override void OnControllerInput(ControllerEvent controllerInput)
	{
	}
}