using UnityEngine;
using System.Collections;

public abstract class BaseScreen : MonoBehaviour
{
	public bool hideCurrent = true; 
	public virtual void OnPush() 
	{
		gameObject.SetActive (true);
	}
	
	public virtual void OnPop() 
	{
		gameObject.SetActive (false);
	}
	
	public virtual void OnLeave(BaseScreen newScreen) 
	{
		if(newScreen.hideCurrent)
		{
			gameObject.SetActive (false);
		}		
	}
	
	public virtual void OnReturn(BaseScreen previousScreen) 
	{
		if (previousScreen.hideCurrent)
		{
			gameObject.SetActive (true);
		}
	}

	public void OnBackButton()
	{
		ScreenManager.instance.Pop();
	}

	void OnEnable()		{ 	ControllerInputManager.OnControllerEvent += OnControllerInput; 	}
	void OnDisable()	{	ControllerInputManager.OnControllerEvent -= OnControllerInput;	}
	
	protected abstract void OnControllerInput(ControllerEvent controllerInput);
}
