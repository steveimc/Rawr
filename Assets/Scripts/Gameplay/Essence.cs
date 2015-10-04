using UnityEngine;
using System.Collections;

public class Essence : MonoBehaviour 
{
	/*
	TODO: 
	Add Particle component
	 */
	public void OnCapture()
	{
		GameManager.Instance.OnEssenceCaptured();
	}
}
