using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class ScreenKeyboard : MonoBehaviour
{
	enum KeyboardMode
	{
		Lowercase,
		Uppercase
	}

	public struct KeyPos
	{
		public KeyPos(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
		public int x;
		public int y;
	}

	//public Button _swapButton;
	public GameObject _alphaButtonsPanel;
	public Text _textField;
	public int _maxChars = 15;

	private KeyboardMode _currentMode;
	private ControllerButton[] _alphaButtons;
	private int _rows = 4;
	private int _columns = 10;

	private KeyPos _currentKeyPos;

	private string _currentText = "";

	private string[,] _lowerChars = new string[4,10]
	{
		{	"a", 	"b", 	"c", 	"d", 	"e", 	"f", 	"g", 		"1", 	"2", "3"},
		{	"h", 	"i",	 "j", 	"k", 	"l", 	"m", "n", 		"4", 	"5", "6"},
		{	"o", 	"p",	 "q", 	"r", 	"s", 	"t",	 "u", 		"7", 	"8", "9"},
		{	"v", 	"w",	 "x", 	"y", 	"z", 	"_",	 "@", 	"-", 	"0", "."}
	};

	private string[,] _upperChars = new string[4,10]
	{
		{	"A", 	"B", 	"C", 	"D", 	"E", 	"F", 	"G", 		"1", 	"2", "3"},
		{	"H", 	"I",	 "J", 	"K", 	"L", 	"M", 	"N", 		"4", 	"5", "6"},
		{	"O", 	"P",	 "Q", "R", 	"S", 	"T",	 "U", 	"7", 	"8", "9"},
		{	"V", 	"W",	 "X", 	"Y", 	"Z", 	"_",	 "@", 	"-", 	"0", "."}
	};

	private Action<string> doneCallback;

	public void Init (String hint, Action<string> callback = null) 
	{
		doneCallback = callback;

		_textField.text = hint;
		_alphaButtons = _alphaButtonsPanel.GetComponentsInChildren<ControllerButton>();
		ChangeMode(KeyboardMode.Uppercase);
		//_swapButton.GetComponentInChildren<Text>().text = KeyboardMode.Uppercase.ToString().ToUpper();

		_currentKeyPos = new KeyPos(0,0);
		_alphaButtons[_currentKeyPos.y* _columns + _currentKeyPos.x].ShowHighlighted();
	}

	void ChangeMode(KeyboardMode newMode)
	{
		_currentMode = newMode;

		string[,] chars = null;
		switch(newMode)
		{
			case KeyboardMode.Lowercase:
				chars = _lowerChars;
				break;
			case KeyboardMode.Uppercase:
				chars = _upperChars;
				break;
		}

		for(int i = 0; i < _rows; i++)
		{
			for(int j = 0; j < _columns; j++)
			{
				_alphaButtons[i * _columns + j].SetText( chars[i, j] );
			}
		}
	}
	
	public void OnSwapButton()
	{
		//_swapButton.GetComponentInChildren<Text>().text = _currentMode.ToString().ToUpper();
		if(_currentMode == KeyboardMode.Lowercase)
			ChangeMode(KeyboardMode.Uppercase);
		else
			ChangeMode(KeyboardMode.Lowercase);
	}

	void OnEnable()		{ 	ControllerInputManager.OnControllerEvent += OnControllerInput; 	}
	void OnDisable()	{	ControllerInputManager.OnControllerEvent -= OnControllerInput;	}

	void OnControllerInput(ControllerEvent controllerInput)
	{
		Debug.Log(controllerInput);
		switch(controllerInput)
		{
			case ControllerEvent.A_Button:
				PressButton();
				break;
			case ControllerEvent.B_Button:
				BackSpace();
				break;
			case ControllerEvent.X_Button:
				EndKeyboardInput();
				break;
			case ControllerEvent.Up:
				SelectButton(NavigateKeys(new KeyPos(0,-1)));
				break;
			case ControllerEvent.Down:
				SelectButton(NavigateKeys(new KeyPos(0,1)));
				break;
			case ControllerEvent.Right:
				SelectButton(NavigateKeys(new KeyPos(1,0)));
				break;
			case ControllerEvent.Left:
				SelectButton(NavigateKeys(new KeyPos(-1,0)));
				break;
		}
	}

	KeyPos NavigateKeys(KeyPos direction)
	{
		KeyPos nextPos = _currentKeyPos;
		nextPos.x += direction.x;
		nextPos.y += direction.y;

		if(nextPos.x > _columns -1)
			nextPos.x = 0;
		else if(nextPos.x < 0)
			nextPos.x = _columns -1;

		if(nextPos.y > _rows -1)
			nextPos.y = 0;
		else if(nextPos.y < 0)
			nextPos.y = _rows -1;

		return nextPos;
	}

	void PressButton()
	{
		_alphaButtons[_currentKeyPos.y* _columns + _currentKeyPos.x].ShowPress();
		Debug.Log(_currentText.Length);
		if(_currentText.Length < _maxChars)
		{
			_currentText += _alphaButtons[_currentKeyPos.y* _columns + _currentKeyPos.x].GetText();
			UpdateTextField();
		}
		else
		{
			// TODO: Error sound
		}
	}

	void BackSpace()
	{
		if(_currentText.Length > 0)
		{
			_currentText = _currentText.Remove(_currentText.Length - 1);
			UpdateTextField();
		}
	}

	void SelectButton(KeyPos pos)
	{
		Debug.Log(_upperChars [pos.y, pos.x]);
		_alphaButtons[_currentKeyPos.y* _columns + _currentKeyPos.x].ShowNormal();
		_alphaButtons[pos.y* _columns + pos.x].ShowHighlighted();
		_currentKeyPos = pos;
	}

	void UpdateTextField()
	{
		_textField.text = _currentText;
	}

	void EndKeyboardInput()
	{
		doneCallback(_currentText);
		ScreenManager.instance.Pop();
	}

}
