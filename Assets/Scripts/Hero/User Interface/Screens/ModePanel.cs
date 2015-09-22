using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ModePanel : MonoBehaviour
{
	[SerializeField] MenuButton[] buttons;
	[SerializeField] MenuTip[] buttonTips;
	[SerializeField] MenuOnline menuOnline;

	int currentlySelected = -1;

	void Start()
	{
		SelectButton(0);
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

		currentlySelected = button;
		buttonTips[currentlySelected].Show();
		EventSystem.current.SetSelectedGameObject(buttons[currentlySelected].gameObject);
	}

	public void PressButton(MenuButton clickedButton)
	{ 
		clickedButton.ShowPress();

		switch(clickedButton.GetButtonType())
		{
			case MenuButtons.SinglePlayer:
				OnSinglePlayerMode();
				break;
			case MenuButtons.Arcade:
				OnArcadeMode();
				break;
			case MenuButtons.Local:
				OnLocalMultiplayer();
				break;
			case MenuButtons.Online:
				OnOnlineMultiplayer();
				break;
			case MenuButtons.Lan:
				OnLanMultiplayer();
				break;
			default:
				Debug.LogWarning("Function not assigned to this button");
				break;
		}
	}

	public void OnSinglePlayerMode()
	{
		Game.Instance.StartAsSinglePlayer();
	}

	public void OnArcadeMode()
	{
		Debug.Log("Arcade");
	}

	public void OnLocalMultiplayer()
	{
		Game.Instance.StartAsLocalGame();;
	}

	public void OnLanMultiplayer()
	{
		Debug.Log("Lan");
	}

	public void OnOnlineMultiplayer()
	{
		Debug.Log("Online");
		UINetworkManager.instance.EnableMatchMaker();
		menuOnline.gameObject.SetActive(true);
	}
}
