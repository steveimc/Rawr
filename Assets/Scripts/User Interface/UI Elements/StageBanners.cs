using UnityEngine;
using System.Collections;

public class StageBanners : MonoBehaviour 
{
	private Animator m_BannersAnimator;

	private const string TRIGGER_STAGE_1 = "Stage_1";
	private const string TRIGGER_STAGE_2 = "Stage_2";
	private const string TRIGGER_STAGE_3 = "Stage_3";
	private const string TRIGGER_STAGE_4 = "Stage_4";
	private const string TRIGGER_STAGE_5 = "Stage_5";
	private const string TRIGGER_STAGE_FINAL = "Stage_6";

	private void Awake()
	{
		m_BannersAnimator = GetComponent<Animator>();
	}

	public void ShowBanner( int stage )
	{
		switch(stage)
		{
		case 1:
			m_BannersAnimator.SetTrigger(TRIGGER_STAGE_1);
			break;
		case 2:
			m_BannersAnimator.SetTrigger(TRIGGER_STAGE_2);
			break;
		case 3:
			m_BannersAnimator.SetTrigger(TRIGGER_STAGE_3);
			break;
		case 4:
			m_BannersAnimator.SetTrigger(TRIGGER_STAGE_4);
			break;
		case 5:
			m_BannersAnimator.SetTrigger(TRIGGER_STAGE_5);
			break;
		case 6:
			m_BannersAnimator.SetTrigger(TRIGGER_STAGE_FINAL);
			break;
		}
	}
}
