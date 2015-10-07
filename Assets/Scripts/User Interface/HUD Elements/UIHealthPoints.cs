using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIHealthPoints : MonoBehaviour 
{
	[SerializeField] Image[] m_imgHpArray;
	[SerializeField] Sprite spr_LifeTrue;
	[SerializeField] Sprite spr_LifeFalse;

	private void Awake()
	{
		//m_imgHpArray = GetComponentsInChildren<Image>();	
	}
	private void Start()
	{
		for(int i = 0; i < m_imgHpArray.Length; i++)
		{
			m_imgHpArray[i].sprite = spr_LifeTrue;
		}
	}

	public void UpdateLife(int newHp)
	{
		if(newHp >= 0)
			m_imgHpArray[newHp].sprite = spr_LifeFalse;
	}

	public void RestoreFullLife()
	{
		for(int i = 0; i < m_imgHpArray.Length; i++)
		{
			m_imgHpArray[i].sprite = spr_LifeTrue;
		}
	}
}
