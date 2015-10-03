using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking.Match;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class MenuOnline : BaseScreen 
{
	[SerializeField] private InputField matchNameInput;
	bool _receiveEvents = true;

	[SerializeField] private ControllerUIElement[] _uiElements;
	int _currentlySelected = -1;
	ControllerUIElements _currentSelectedType = ControllerUIElements.None;

	void Start()
	{
		SelectElement(0);
		//_receiveEvents = false;
		//ScreenManager.instance.Push<KeyboardPopup>().Init("Game Name", OnKeyboardInputEnd);
	}

	private void OnKeyboardInputEnd(string hostIp)
	{
		_receiveEvents = true;
		matchNameInput.text = hostIp;
	}

	protected override void OnControllerInput(ControllerEvent controllerInput)
	{
		if( _receiveEvents == false)
			return;
		
		switch(controllerInput)
		{
		case ControllerEvent.Up:
			SelectElement(_currentlySelected - 1);
			break;
		case ControllerEvent.Down:
			SelectElement(_currentlySelected +1);
			break;
		case ControllerEvent.A_Button:
			PressElement();
			break;
		case ControllerEvent.X_Button:
			ScreenManager.instance.Pop();
			break;
		}
	}

	
	private void DeselectCurrent()
	{
		if(_currentSelectedType == ControllerUIElements.InputField)
		{
			EventSystem.current.SetSelectedGameObject(null);
		}
		else if (_currentSelectedType == ControllerUIElements.ControllerButton)
		{
			_uiElements[_currentlySelected].GetComponent<ControllerButton>().ShowNormal();
		}
	}
	
	private void SelectElement(int index)
	{
		if(_currentlySelected != -1)
		{
			DeselectCurrent();
		}
		
		if(index < 0)
		{
			index = _uiElements.Length  - 1;
		}
		else if( index == _uiElements.Length)
		{
			index = 0;
		}
		
		_currentlySelected = index;
		_currentSelectedType = _uiElements[_currentlySelected].GetElementType();
		if(_currentSelectedType == ControllerUIElements.InputField)
		{
			FocusInputField(_uiElements[_currentlySelected].GetComponent<InputField>());
		}
		else if (_currentSelectedType == ControllerUIElements.ControllerButton)
		{
			_uiElements[_currentlySelected].GetComponent<ControllerButton>().ShowHighlighted();
		}
	}
	
	private void PressElement()
	{
		if(_currentSelectedType == ControllerUIElements.InputField)
		{
			_receiveEvents = false;
			ScreenManager.instance.Push<KeyboardPopup>().Init("localhost", OnKeyboardInputEnd);
		}
		else if (_currentSelectedType == ControllerUIElements.ControllerButton)
		{
			ControllerButton btn = _uiElements[_currentlySelected].GetComponent<ControllerButton>();
			btn.ShowPress();
			btn.DoClick();
		}
	}
	
	void FocusInputField( InputField textInput)
	{
		EventSystem.current.SetSelectedGameObject(textInput.gameObject, null);
		textInput.OnPointerClick(new PointerEventData(EventSystem.current));
	}


	public void OnClickCreateMatchmakingGame()
	{
		UINetworkManager.instance.SetMatch(matchNameInput.text, 2);
		UINetworkManager.instance.CreateInternetMatch();
		/*
		NetworkManager.singleton.StartMatchMaker();
		CreateMatchRequest matchOptions = new CreateMatchRequest()
		{
			name = matchNameInput.text,
			size =  (uint)NetworkManager.singleton.maxConnections,
			advertise = true,
			password = "",
			privateAddress = "192.168.0.105"
		};

		NetworkManager.singleton.matchMaker.CreateMatch(matchOptions, NetworkManager.singleton.OnMatchCreate);

		/* 		lobbyManager.matchMaker.CreateMatch(
                matchNameInput.text,
                (uint)lobbyManager.maxPlayers,
                true,
                "",
                lobbyManager.OnMatchCreate);*/
		
		//NetworkManager.singleton.backDelegate = NetworkManager.singleton.StopHost;
		//NetworkManager.singleton.isMatchmaking = true;
		//NetworkManager.singleton.DisplayIsConnecting();
	}

	public void OnClickOpenServerList()
	{
		UINetworkManager.instance.FindInternetMatch();
		//NetworkManager.singleton.StartMatchMaker();
		//NetworkManager.singleton.backDelegate = lobbyManager.SimpleBackClbk;
		//NetworkManager.singleton.ChangeTo(lobbyServerList);
	}
}
