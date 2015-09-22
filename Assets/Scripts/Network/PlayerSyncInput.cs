using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerSyncInput : NetworkBehaviour 
{
	struct UserInput
	{
		public double time;
		public float 	x;
		public float 	y;
		public bool 	j;
		public bool 	d;
	}
	// float fHorizontal, float fVertical, bool bJump, bool bDash

	[ SyncVar (hook = "SyncUserInput") ] private UserInput syncUserInput;

	private HeroBaseController myHeroController;
	private NetworkClient networkClient;
	private float latency;
	private Text latencyText;
	private float lerpRate = 15.0f;

	private List<UserInput> userInputList = new List<UserInput>() ;

	// Use this for initialization
	void Start () 
	{
		if(Game.Instance.IsLocalGame)
			return;

			networkClient = GameObject.Find("NetworkManager").GetComponent<NetworkManager>().client;
			latencyText = GameObject.Find("LatencyText").GetComponent<Text>();

			if(!isLocalPlayer)
			{
				GetComponent<UserControls>().enabled = false;
				// This will prevent it from calculating physics. If it wont ever be needed we should just remove it
				//GetComponent<Rigidbody2D>().isKinematic = true; 
				GetComponent<Renderer>().material.color = Color.red;
				myHeroController = GetComponent<HeroBaseController>();
			}
			else if(isLocalPlayer)
			{
				GetComponent<Renderer>().material.color = Color.blue;
				FindObjectOfType<CameraController>().Init(this.transform);
			}

			if(isServer)
				GameObject.Find("ServerOrClient").GetComponent<Text>().text = "Server"; 
		}
	
	// Update is called once per frame
	void Update () 
	{
		if(Game.Instance.IsLocalGame)
			return;

		ShowLatency ();
		MovePlayer();
	}

	// Time delta time for fixed update is constant
	void FixedUpdate () 
	{
		if(Game.Instance.IsLocalGame)
			return;
		
		//SendInput ();
	}

	[ClientCallback] 
	public void SendInput(float fHorizontal, float fVertical, bool bJump, bool bDash)
	{
		if(isLocalPlayer)
		{
			UserInput lastInput = new UserInput();
			lastInput.time = Time.timeSinceLevelLoad;
			lastInput.x = fHorizontal;
			lastInput.y = fVertical;
			lastInput.j = bJump;
			lastInput.d = bDash;

			CmdSendInputToServer(lastInput);
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
		
		if(!isLocalPlayer)
			userInputList.Add (syncUserInput);
	}

	void MovePlayer()
	{
		if(!isLocalPlayer && userInputList.Count > 0)
		{
			if(userInputList[0].time + 0.1f >= Time.timeSinceLevelLoad)
			{
				ApplyInput(userInputList[0]);
				userInputList.RemoveAt(0);
			}
		}
	}

	void ApplyInput(UserInput input)
	{
		myHeroController.Move(input.x, input.y, input.j, input.d );
	}

	void ShowLatency()
	{
		if(isLocalPlayer)
		{
			latency = networkClient.GetRTT();
			latencyText.text = latency.ToString();
		}
	}
}
