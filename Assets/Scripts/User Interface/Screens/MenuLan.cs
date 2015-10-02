using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MenuLan : BaseScreen 
{
	[SerializeField] private InputField hostIPInput;
	bool _receiveEvents = true;

	void Start()
	{
		_receiveEvents = false;
		hostIPInput.text = "localhost";
		ScreenManager.instance.Push<KeyboardPopup>().Init(OnKeyboardInputEnd);
	}

	protected override void OnControllerInput(ControllerEvent controllerInput)
	{
		if( _receiveEvents == false)
			return;

		switch(controllerInput)
		{
		case ControllerEvent.X_Button:
			ScreenManager.instance.Pop();
			break;
		}
	}

	private void OnKeyboardInputEnd(string hostIp)
	{
		_receiveEvents = true;
		hostIPInput.text = hostIp;
	}

	public void OnHostGame()
	{
		UINetworkManager.instance.StartHost();
	}

	public void OnJoinGame()
	{
		UINetworkManager.instance.SetLanHostAddress(hostIPInput.text);
	}
}
