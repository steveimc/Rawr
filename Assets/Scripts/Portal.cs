using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour 
{
	private Portal[] m_Portals;
	private bool m_bTeleport = true;

	// Use this for initialization
	void Awake() 
	{
		m_Portals = FindObjectsOfType<Portal>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnTriggerEnter2D(Collider2D col2D)
	{
		if(!m_bTeleport)
			return;
		
		if(col2D.gameObject.GetComponent<HeroStatus>() )
		{
			int iRandom = SelectRandom();

			Vector3 portalPosition = m_Portals[iRandom].transform.position;
			m_Portals[iRandom].SetTeleport(false);
			col2D.transform.position = portalPosition;
		}
		else if(col2D.gameObject.GetComponent<EnemySlug>())
		{
			int iRandom = SelectRandom();

			Vector3 portalPosition = m_Portals[iRandom].transform.position;
			m_Portals[iRandom].SetTeleport(false);
			col2D.transform.position = portalPosition;
			col2D.transform.rotation = m_Portals[iRandom].transform.rotation;
		}
	}

	private void OnTriggerExit2D(Collider2D col2D)
	{
		if(col2D.gameObject.GetComponent<HeroStatus>())
		{
			SetTeleport(true);
		}
		else if(col2D.gameObject.GetComponent<EnemySlug>())
		{
			SetTeleport(true);
		}
	}

	private int SelectRandom()
	{
		int iRandom = 0;

		for(int i = 0; i < m_Portals.Length; i++)
		{
			iRandom = Random.Range(0,m_Portals.Length);

			while(m_Portals[iRandom] == this)
			{
				iRandom = Random.Range(0,m_Portals.Length);
			}
		}

		return iRandom;
	}

	public void SetTeleport(bool bStatus)
	{
		m_bTeleport = bStatus;
	}
}
