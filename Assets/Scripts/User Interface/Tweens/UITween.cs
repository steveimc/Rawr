using UnityEngine;
using System.Collections;
using DG.Tweening;

// abstract class can never be used by itself
public abstract class UITween : MonoBehaviour 
{
	public float duration;
	public Ease easeType;
	private Tweener tweener;
	
	// Use this for initialization
	protected virtual void Awake () 
	{
		tweener = SetupTween();
		tweener.SetEase(easeType);
		tweener.SetAutoKill(false);
		tweener.Pause();
	}
	
	private void OnEnable()
	{
		tweener.Restart();
		tweener.PlayForward();
	}
	//Forces an extending class to implement it
	protected abstract Tweener SetupTween();
	
}
