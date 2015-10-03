using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class MenuLan : BaseScreen 
{
	[SerializeField] private InputField hostIPInput;
	bool _receiveEvents = true;

	[SerializeField] private ControllerUIElement[] _uiElements;
	int _currentlySelected = -1;
	ControllerUIElements _currentSelectedType = ControllerUIElements.None;

	void Start()
	{
		hostIPInput.text = "localhost";
		SelectElement(0);
		//ScreenManager.instance.Push<KeyboardPopup>().Init("Insert Host IP Address", OnKeyboardInputEnd);
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
