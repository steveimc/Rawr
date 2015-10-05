using UnityEngine;
using System.Collections;
using System;

public class Stage
{
	private int m_iStageNumber;
	private int m_iEssencesToPass;
	private int m_iCurrentEssences = 0;

	private int m_enemyTypes = 0;

	private Action<int> m_stageClearedCallback;

	public Stage(Action<int> stageClearedCallback)
	{
		m_stageClearedCallback = stageClearedCallback;
	}

	public int GetCurrentStage()
	{
		return m_iStageNumber;
	}

	public int GetEnemyTypes()
	{
		return m_enemyTypes;
	}

	public void Init(int stageNumber)
	{
		m_iStageNumber 	= stageNumber;
		HUDController.instance.ShowStageBanner(m_iStageNumber);
		m_iEssencesToPass 	= EssencesForStageMultiplier(m_iStageNumber);
		if(m_iStageNumber < 3)
			m_enemyTypes = m_iStageNumber +1;
	}

	private void StageCleared()
	{
		m_stageClearedCallback(m_iStageNumber);
	}

	public void UpdateEssences(int currentEssences)
	{
		m_iCurrentEssences = currentEssences;
		if(m_iCurrentEssences >= m_iEssencesToPass)
		{
			StageCleared();
		}
	}

	// Change the formula to adjust target essences per stage
	private int EssencesForStageMultiplier(int stage)
	{
		int numOfEssences = Mathf.FloorToInt( ((stage + 2) * (stage + 1)) * 1.5f); 
		// 0 -> 2*1 = 2 * 1.5 = 3
		// 1 -> (1+2)*(1+1) = 6 = 9
		// 2 -> (2+2) * (2+1) = 12 = 18
		// 3 -> (3+2) * (3+1) = 20 = 30 
		// 4 -> (4+2) * (4+1) = 30 = 45
		return numOfEssences;
	}
	
}
