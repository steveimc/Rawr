using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MenuLan : BaseScreen 
{
	[SerializeField] private InputField hostIPInput;

	void Start()
	{
		hostIPInput.text = "localhost";
	}

	public void OnHostGame()
	{
		UINetworkManager.instance.StartHost();
	}

	public void OnJoinGame()
	{
		UINetworkManager.instance.SetLanHostAddress(hostIPInput.text);
	}
}
