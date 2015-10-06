using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIToggleImage : MonoBehaviour 
{
	Image m_ThisImage; 
	[SerializeField] Sprite spriteTrue;
	[SerializeField] Sprite spriteFalse;

	internal bool m_State;

	// Use this for initialization
	void Start () 
	{
		m_ThisImage = GetComponent<Image>();
		m_State = false;
		m_ThisImage.sprite = spriteFalse;
	}
	
	public void Toggle(bool state)
	{
		if(state != m_State)
		{
			m_State = state;
			if(m_State)
				m_ThisImage.sprite = spriteTrue;
			else
				m_ThisImage.sprite = spriteFalse;
		}
	}
	
}
