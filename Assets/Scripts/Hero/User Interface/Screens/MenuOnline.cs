using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking.Match;
using UnityEngine.Networking;

public class MenuOnline : MonoBehaviour 
{
	[SerializeField] private InputField matchNameInput;


	public void OnClickCreateMatchmakingGame()
	{
		UINetworkManager.instance.SetMatch(matchNameInput.text, 2);
		UINetworkManager.instance.CreateInternetMatch();
		/*
		NetworkManager.singleton.StartMatchMaker();
		CreateMatchRequest matchOptions = new CreateMatchRequest()
		{
			name = matchNameInput.text,
			size =  (uint)NetworkManager.singleton.maxConnections,
			advertise = true,
			password = "",
			privateAddress = "192.168.0.105"
		};

		NetworkManager.singleton.matchMaker.CreateMatch(matchOptions, NetworkManager.singleton.OnMatchCreate);

		/* 		lobbyManager.matchMaker.CreateMatch(
                matchNameInput.text,
                (uint)lobbyManager.maxPlayers,
                true,
                "",
                lobbyManager.OnMatchCreate);*/
		
		//NetworkManager.singleton.backDelegate = NetworkManager.singleton.StopHost;
		//NetworkManager.singleton.isMatchmaking = true;
		//NetworkManager.singleton.DisplayIsConnecting();
	}

	public void OnClickOpenServerList()
	{
		UINetworkManager.instance.FindInternetMatch();
		//NetworkManager.singleton.StartMatchMaker();
		//NetworkManager.singleton.backDelegate = lobbyManager.SimpleBackClbk;
		//NetworkManager.singleton.ChangeTo(lobbyServerList);
	}
}
