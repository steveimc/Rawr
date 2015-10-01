using UnityEngine;
using System.Collections;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class UIFadeTween : UITween 
{
	public float startValue = 0;
	private float endValue = 1;
	
	private CanvasGroup canvasGroup;
	protected override void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		endValue = canvasGroup.alpha;
		canvasGroup.alpha = startValue;
		
		base.Awake();
	}
	protected override Tweener SetupTween()
	{
		return canvasGroup.DOFade(endValue, duration);
	}
}
