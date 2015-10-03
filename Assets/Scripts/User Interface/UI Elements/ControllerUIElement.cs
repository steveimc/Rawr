using UnityEngine;
using System.Collections;

public enum ControllerUIElements
{
	None = -1,
	ControllerButton, 
	InputField
}

public class ControllerUIElement : MonoBehaviour 
{
	[SerializeField] private ControllerUIElements elementType;

	public ControllerUIElements GetElementType()
	{
		return elementType;
	}
}
