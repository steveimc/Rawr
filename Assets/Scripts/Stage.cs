using UnityEngine;
using System.Collections;
using System;

public class Stage : MonoBehaviour 
{
	private int m_iStageNumber;
	private int m_iEssencesToPass;
	private int m_iCurrentEssences = 0;

	private Action<int> m_stageClearedCallback;

	public Stage(Action<int> stageClearedCallback)
	{
		m_stageClearedCallback = stageClearedCallback;
	}

	public void Init(int stageNumber, int essencesToPass )
	{
		m_iStageNumber 		= stageNumber;
		m_iEssencesToPass 	= essencesToPass;
	}

	private void StageCleared()
	{
		m_stageClearedCallback(m_iStageNumber);
	}

	public void UpdateEssences(int currentEssences)
	{
		m_iCurrentEssences = currentEssences;
	}
	
}
