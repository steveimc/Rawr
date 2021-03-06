﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public enum UserInput
{
	Jump, 
	Dash,
	Attack1,
	Attack2,
	Attack3,
	Crouch,
	CrouchStop,
	ToggleSword,
	ToggleSwordON,
	ToggleSwordOFF,
	WallJump
}

public class PlayerSyncInput : NetworkBehaviour 
{
	[ SyncVar (hook = "SyncUserInput") ] private UserInput syncUserInput;

	public HeroStatus heroStatus;

	private AnimationController heroAnimator;
	private NetworkClient networkClient;
	private float latency;
	private Text latencyText;
//	private float lerpRate = 15.0f;

//	private List<UserInput> userInputList = new List<UserInput>() ;

	// Use this for initialization
	void Start () 
	{
		if(Game.Instance.IsLocalGame)
			return;

		heroAnimator 		= GetComponent<AnimationController>();
		//heroStatus 				= GetComponent<HeroStatus>();
	}

	[ClientCallback] 
	public void SendInput(UserInput input)
	{
		if(isLocalPlayer)
		{
			CmdSendInputToServer(input);
		}
	}

	[Command] 
	void CmdSendInputToServer(UserInput input)
	{
		syncUserInput = input;
	}
	
	[Client]
	void SyncUserInput(UserInput latestInput)
	{
		syncUserInput = latestInput;

		ApplyInput(syncUserInput);
	}

	void ApplyInput(UserInput input)
	{
		switch(input)
		{
			case UserInput.Jump:
				heroAnimator.Jump();
				break;
			case UserInput.Dash:
				heroAnimator.Dash();
				break;
			case UserInput.Crouch:
				heroAnimator.Crouch(-10.0f);
				break;
			case UserInput.CrouchStop:
				heroAnimator.Crouch(10.0f);
				break;
			case UserInput.Attack1:
				heroAnimator.Attack(0);
				break;
			case UserInput.Attack2:
				heroAnimator.Attack(1);
				break;
			case UserInput.Attack3:
				heroAnimator.Attack(2);
				break;	
			/*
			 * case UserInput.ToggleSword:
					heroStatus.ToggleSwordFromNetwork();
				break;
			 */
			case UserInput.ToggleSwordON:
				heroStatus.ToggleSwordFromNetwork(true);
				break;
			case UserInput.ToggleSwordOFF:
				heroStatus.ToggleSwordFromNetwork(false);
				break;
			case UserInput.WallJump:
				Debug.Log("wall jump");
				break;
			default:
				Debug.LogWarning("Input not recognized");
				break;
		}
	}
}
