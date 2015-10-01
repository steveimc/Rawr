using UnityEngine;
using System.Collections;
using DG.Tweening;

public class UIPositionTween : UITween 
{
	public Vector3 startOffset;
	private Vector3 endPos;
	
	protected override void Awake()
	{
		endPos = GetComponent<RectTransform>().localPosition;
		GetComponent<RectTransform>().localPosition = endPos + startOffset;
		base.Awake();
	}
	
	protected override Tweener SetupTween()
	{
		return GetComponent<RectTransform>().DOLocalMove(endPos, duration);
	}
	
	
	private void OnDrawGizmosSelected()
	{
		if(!Application.isPlaying)
			Gizmos.DrawLine(transform.position, transform.position+transform.TransformVector(startOffset));
	}
}
