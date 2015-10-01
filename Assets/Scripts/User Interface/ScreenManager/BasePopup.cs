using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
public class BasePopup : BaseScreen 
{

	public Text descriptionText;
	public Button cancelButton;
	public Button okButton;
	
	private Action okCallback;
	
	private void Awake()
	{
		cancelButton.onClick.AddListener(OnCancelButton);
		okButton.onClick.AddListener(OnOKButton);
	}
	
	//Description and what should happen when the player clicks
	public void Init (string description, Action callback = null)
	{
		descriptionText.text = description;
		okCallback = callback;
	}
	
	private void OnCancelButton()
	{
		ScreenManager.instance.Pop();
	}
	
	private void OnOKButton()
	{
		ScreenManager.instance.Pop();
		if(okCallback != null)
		{
			okCallback();
		}
	}

	protected override void OnControllerInput(ControllerEvent controllerInput)
	{
		switch(controllerInput)
		{
		case ControllerEvent.B_Button:
			ScreenManager.instance.Pop();
			break;
		}
	}
}
