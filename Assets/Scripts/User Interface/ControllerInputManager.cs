using UnityEngine;
using System.Collections;
using System;

public enum ControllerEvent
{
	Up,
	Down,
	Left,
	Right, 
	A_Button,
	X_Button,
	B_Button,
	Y_Button,
	Back,
	Start
}

public class ControllerInputManager : MonoBehaviour 
{
	public static event Action<ControllerEvent>  OnControllerEvent;
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetButtonDown(InputBank.BUTTON_A))
		{
			SendControllerEvent(ControllerEvent.A_Button);
		}

		if(Input.GetButtonDown(InputBank.BUTTON_X))
		{
			SendControllerEvent(ControllerEvent.X_Button);
		}

		if(Input.GetButtonDown(InputBank.BUTTON_B))
		{
			SendControllerEvent(ControllerEvent.B_Button);
		}

		if(Input.GetButtonDown(InputBank.BUTTON_Y))
		{
			SendControllerEvent(ControllerEvent.Y_Button);
		}

		float fVertical = Input.GetAxis(InputBank.VERTICAL+1);
		float fHorizontal = Input.GetAxis(InputBank.HORIZONTAL+1);

		if(fVertical > 0.1)
		{
			if(!m_doVertical)
				SendControllerEvent(ControllerEvent.Up);
			m_doVertical = true;
		}
		else if(fVertical < -0.1)
		{
			if(!m_doVertical)
				SendControllerEvent(ControllerEvent.Down);
			m_doVertical = true;
		}
		else if(fHorizontal > 0.1 )
		{
			if(!m_doHorizontal)
				SendControllerEvent(ControllerEvent.Right);
			m_doHorizontal = true;
		}
		else if(fHorizontal < -0.1 )
		{
			if(!m_doHorizontal)
				SendControllerEvent(ControllerEvent.Left);
			m_doHorizontal = true;
		}
		else
		{
			m_doVertical = false;
			m_doHorizontal = false;
		}
	}
	bool m_doVertical = false;
	bool m_doHorizontal = false;
	
	void SendControllerEvent(ControllerEvent controllerEvent)
	{
		if(OnControllerEvent != null)
			OnControllerEvent(controllerEvent);
	}
}