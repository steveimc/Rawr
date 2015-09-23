using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerSyncPosition : NetworkBehaviour 
{
	[SyncVar (hook = "SyncPositionValues") ] private Vector3 syncPos; // This will automatically replace for this object in all clients
	// hook: instead of replacing variable, calls this function and giver var as param

	[SerializeField] private HeroBaseController heroController;
	[SerializeField] private Transform thisTransform;
						
	[SerializeField] private float normalLerpRate = 20;
	[SerializeField] private float fasterLerpRate = 30;
	private float lerpRate = 15f;

	private Vector3 lastPos;
	private float distanceThreshold = 0.02f;
	
	private NetworkClient networkClient;
	private int latency;
	private Text latencyText;

	private List<float> timeStamps = new List<float>();
	[SerializeField] private bool useHistoricalLerp = false;

	private List<Vector3> syncPosList = new List<Vector3>();
	[SerializeField] private bool useTimelineLerp = false;
	[SerializeField] private float closeEnough = 0.005f;
	
	void Start()
	{
		if(Game.Instance.IsLocalGame)
			return;

			networkClient = GameObject.Find("NetworkManager").GetComponent<NetworkManager>().client;
			latencyText = GameObject.Find("LatencyText").GetComponent<Text>();
			lerpRate = normalLerpRate;
			heroController = GetComponent<HeroBaseController>();

			if(!isLocalPlayer)
			{
				GetComponent<UserControls>().enabled = false;
				// This will prevent it from calculating physics. If it wont ever be needed we should just remove it
				GetComponent<Rigidbody2D>().isKinematic = true; 
			}
			else if(isLocalPlayer)
			{
				FindObjectOfType<CameraController>().Init(this.transform);
			}

			if(isServer)
				GameObject.Find("ServerOrClient").GetComponent<Text>().text = "Server";
	}
	
	// Time delta.time varies according to framerate
	void Update()
	{
		if(Game.Instance.IsLocalGame)
			return;

		LerpPosition ();
		ShowLatency ();
	}
	
	// Time delta time for fixed update is constant
	void FixedUpdate () 
	{
		if(Game.Instance.IsLocalGame)
			return;

		SendPosition ();
	}
	
	void LerpPosition()
	{
		if ( !isLocalPlayer) 
		{
			if (useHistoricalLerp) 
				HistoricalLerp();
			else if(useTimelineLerp)
				TimeLineLerp();
			else 
				NormalLerp ();
		}
	}
	
	// Commands must start by Cmd. Called by client but Will run only on server.
	[Command] 
	void CmdSendPositionToServer(Vector3 pos)
	{
		// In the server syncPos will be pos
		syncPos = pos;
	}
	
	[ClientCallback] 
	void SendPosition()
	{
		if(isLocalPlayer && Vector3.Distance(thisTransform.position, lastPos) > distanceThreshold)
		{
			CmdSendPositionToServer (thisTransform.position);
			lastPos = thisTransform.position;
			Debug.Log("pos sent");
		}
	}

	[SerializeField] private float lag = 1.0f;

	[Client]
	void SyncPositionValues(Vector3 latestPos)
	{
		syncPos = latestPos;
		heroController.m_Facing = heroController.m_animator.CheckDirection();
		heroController.Flip(this.transform);

		if(!isLocalPlayer)
		{
			syncPosList.Add (syncPos);
			timeStamps.Add(Time.timeSinceLevelLoad + lag);
		}
	}
	
	void ShowLatency()
	{
		if(isLocalPlayer)
		{
			latency = networkClient.GetRTT();
			latencyText.text = latency.ToString() + " / Sync Pos Count: " + syncPosList.Count;
		}
	}
	
	void NormalLerp()
	{
		thisTransform.position = Vector3.Lerp(thisTransform.position, syncPos, Time.deltaTime * lerpRate);
	}

	void HistoricalLerp()
	{
		if (syncPosList.Count > 0) 
		{
			this.transform.position = Vector3.Lerp(thisTransform.position, syncPosList[0], Time.deltaTime * lerpRate);
			
			if(Vector3.Distance(thisTransform.position, syncPosList[0]) < closeEnough)
			{
				syncPosList.RemoveAt(0);
			}

			if(syncPosList.Count > 10)
				lerpRate = fasterLerpRate;
			else
				lerpRate = normalLerpRate;
		}
	}

	void TimeLineLerp()
	{
		if(!isLocalPlayer && syncPosList.Count > 0)
		{
			if(timeStamps[0] >= Time.timeSinceLevelLoad)
			{
				this.transform.position = Vector3.Lerp(thisTransform.position, syncPosList[0], Time.deltaTime * lerpRate);

				if(Vector3.Distance(thisTransform.position, syncPosList[0]) < closeEnough || ( timeStamps.Count > 1 && timeStamps[1] >= Time.timeSinceLevelLoad))
				{
					syncPosList.RemoveAt(0);
					timeStamps.RemoveAt(0);
				}

				if(syncPosList.Count > 10)
					lerpRate = fasterLerpRate;
				else
					lerpRate = normalLerpRate;
			}/*
			else
			{
				syncPosList.RemoveAt(0);
				timeStamps.RemoveAt(0);
			}*/
		}
	}

}

/*
	struct PosPacket
	{
		public uint c; // order
		public double t; // time
		public Vector3 pos; // position
	}*/
//[SyncVar ] private PosPacket syncPosPacket;

