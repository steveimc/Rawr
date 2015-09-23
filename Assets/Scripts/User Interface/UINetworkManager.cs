using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class UINetworkManager : MonoBehaviour 
{
	private NetworkManager networkManager;

	private static UINetworkManager _instance;
	public static UINetworkManager instance
	{
		get
		{											
			if (_instance == null)
				_instance = (UINetworkManager)FindObjectOfType (typeof (UINetworkManager));			

			return _instance;
		}
	}
	
	// Use this for initialization
	void Start () 
	{
		networkManager = FindObjectOfType<NetworkManager>();
	}

	public void StartServer()
	{
		if (!NetworkClient.active && !NetworkServer.active && networkManager.matchMaker == null)
			networkManager.StartServer();
	}

	public void StartHost()
	{
		if (!NetworkClient.active && !NetworkServer.active && networkManager.matchMaker == null)
			networkManager.StartHost();
	}

	public void StartClient()
	{
		if (!NetworkClient.active && !NetworkServer.active && networkManager.matchMaker == null)
			networkManager.StartClient();
	}

	public void StopHost()
	{
		if (NetworkServer.active && NetworkClient.active)
			networkManager.StopHost();
	}

	public void SetLanHostAddress(string ip)
	{
		networkManager.networkAddress = ip;
	}

	public void ClientReady()
	{
		if (NetworkClient.active && !ClientScene.ready)
		{
			ClientScene.Ready(networkManager.client.connection);

			if(ClientScene.localPlayers.Count == 0)
				ClientScene.AddPlayer(0);
		}
	}

	public void EnableMatchMaker()
	{
		if (!NetworkServer.active && !NetworkClient.active)
			networkManager.StartMatchMaker();
	}

	public void CreateInternetMatch()
	{
		networkManager.matchMaker.CreateMatch("networkManager.matchName", networkManager.matchSize, true, "", networkManager.OnMatchCreate);
	}

	public void FindInternetMatch()
	{
		networkManager.matchMaker.ListMatches(0,20,"", networkManager.OnMatchList);
	}

	public void SetMatch(string matchName, int numOfPlayers)
	{
		networkManager.matchName = matchName;
		networkManager.matchSize = (uint)numOfPlayers;
	}

	public void JoinMatch(MatchDesc match)
	{
		networkManager.matchName = match.name;
		networkManager.matchSize = (uint)match.currentSize;
		networkManager.matchMaker.JoinMatch(match.networkId, "", networkManager.OnMatchJoined);
	}

	public void OnLocal()
	{
		networkManager.SetMatchHost("localhost", 7777, false);
	}

	public void OnOnline()
	{
		networkManager.SetMatchHost("mm.unet.unity3d.com", 443, true);
	}

	public void DisableMatchMaker()
	{
		networkManager.StopMatchMaker();
	}

	void OnGUI()
	{	
		int xpos = 10 ;
		int ypos = 40 ;
		int spacing = 24;

		if (networkManager.matchInfo == null)
		{
			if (networkManager.matches != null)
			{
				foreach (var match in networkManager.matches)
				{
					if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Join Match:" + match.name))
					{
						networkManager.matchName = match.name;
						networkManager.matchSize = (uint)match.currentSize;
						networkManager.matchMaker.JoinMatch(match.networkId, "", networkManager.OnMatchJoined);
					}
					ypos += spacing;
				}
			}
		}
	}
}
