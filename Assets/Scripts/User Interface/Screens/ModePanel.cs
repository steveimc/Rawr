using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ModePanel : BaseScreen
{
	[SerializeField] MenuButton[] buttons;
	[SerializeField] MenuTip[] buttonTips;
	[SerializeField] MenuOnline menuOnline;

	int currentlySelected = -1;

	void Start()
	{
		SelectButton(0);
	}

	protected override void OnControllerInput(ControllerEvent controllerInput)
	{
		switch(controllerInput)
		{
		case ControllerEvent.Up:
			if(currentlySelected  == 0)
			{
				SelectButton(buttons.Length -1);
			}
			else
			{
				SelectButton(currentlySelected - 1);
			}
			break;
		case ControllerEvent.Down:
			if(currentlySelected == buttons.Length -1)
			{
				SelectButton(0);
			}
			else
			{
				SelectButton(currentlySelected + 1);
			}
			break;
		case ControllerEvent.A_Button:
			PressButton(buttons[currentlySelected]);
			break;
		}
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			if(currentlySelected  == 0)
			{
				SelectButton(buttons.Length -1);
			}
			else
			{
				SelectButton(currentlySelected - 1);
			}
		}
		else if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			if(currentlySelected == buttons.Length -1)
			{
				SelectButton(0);
			}
			else
			{
				SelectButton(currentlySelected + 1);
			}
		}

		if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			PressButton(buttons[currentlySelected]);
		}
	}

	void SelectButton(int button)
	{
		if(currentlySelected != -1)
		{
			buttonTips[currentlySelected].Hide();
		}

		AudioManager.instance.Play(Audio.Bank.UI_HOVER);
		currentlySelected = button;
		buttonTips[currentlySelected].Show();
		EventSystem.current.SetSelectedGameObject(buttons[currentlySelected].gameObject);
	}

	public void PressButton(MenuButton clickedButton)
	{ 
		clickedButton.ShowPress();

		switch(clickedButton.GetButtonType())
		{
			case GameMode.SinglePlayer:
				OnSinglePlayerMode();
				break;
			case GameMode.Arcade:
				OnArcadeMode();
				break;
			case GameMode.Local:
				OnLocalMultiplayer();
				break;
			case GameMode.Online:
				OnOnlineMultiplayer();
				break;
			case GameMode.Lan:
				OnLanMultiplayer();
				break;
			default:
				Debug.LogWarning("Function not assigned to this button");
				break;
		}
	}

	public void OnSinglePlayerMode()
	{
		AudioManager.instance.Play(Audio.Bank.UI_ACCEPT);
		Game.Instance.StartAsSinglePlayer();
	}

	public void OnArcadeMode()
	{
		AudioManager.instance.Play(Audio.Bank.UI_BACK);
		Debug.Log("Arcade Coming soon");
	}

	public void OnLocalMultiplayer()
	{
		AudioManager.instance.Play(Audio.Bank.UI_ACCEPT);
		Game.Instance.StartAsLocalGame();;
	}

	public void OnLanMultiplayer()
	{
		AudioManager.instance.Play(Audio.Bank.UI_BACK);
		Debug.Log("Lan Multiplayer Coming soon");
		// ScreenManager.instance.Push<MenuLan>();
	}

	public void OnOnlineMultiplayer()
	{
		AudioManager.instance.Play(Audio.Bank.UI_BACK);
		Debug.Log("Online Multiplayer Coming soon");
		//UINetworkManager.instance.EnableMatchMaker();
		// ScreenManager.instance.Push<MenuOnline>();
	}
}
