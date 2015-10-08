using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserControls : MonoBehaviour 
{

	private HeroBaseController m_Controller;
	private AnimationController m_AnimationController;

	private HeroStatus m_Hero;

	private bool m_bJump;
	private bool m_bJumpHold;
	private bool m_bDash;
	private bool m_bAttack;
	private bool m_bCharge;

	private void Awake()
	{
		m_Hero = GetComponent<HeroStatus>();
		m_AnimationController = GetComponent<AnimationController>();
	}

	private void Update()
	{
		if(m_Controller == null)
		{
			m_Hero.ChangeHeroController();
			m_Controller = m_Hero.m_Controller;
			m_AnimationController.ChangeAnimator();
		}
		else if(m_Controller != m_Hero.m_Controller)
		{
			m_Controller = m_Hero.m_Controller;
			m_AnimationController.ChangeAnimator();
		}
		else if(m_Hero.m_iHeroHealth > 0)
		{
			if (!m_bJump)
			{
				// Read the jump input in Update so button presses aren't missed.
				m_bJump = Input.GetButtonDown(InputBank.JUMP+m_Hero.m_iHeroId);
				m_bDash = Input.GetButtonDown(InputBank.DASH+m_Hero.m_iHeroId);
				m_bAttack = Input.GetButtonUp(InputBank.ATTACK+m_Hero.m_iHeroId);
				m_bCharge = Input.GetButtonDown(InputBank.ATTACK+m_Hero.m_iHeroId);
			}
				
			m_bJumpHold = Input.GetButton(InputBank.JUMP+m_Hero.m_iHeroId);

			if(Input.GetButtonDown(InputBank.SWORD+m_Hero.m_iHeroId))
			{
				if(!m_Controller.m_isSpinning)
				{
					m_Hero.DropSword();
					m_Hero.ChangeHeroController();
				}

			}

			float fVertical = Input.GetAxis(InputBank.VERTICAL+m_Hero.m_iHeroId);
			float fHorizontal = Input.GetAxis(InputBank.HORIZONTAL+m_Hero.m_iHeroId);

			fVertical = fVertical * 0.8f;

			m_Controller.Move(fHorizontal, fVertical, m_bJump, m_bDash, m_bJumpHold);
			m_Controller.Attack(fHorizontal, m_bAttack, m_bCharge);

			m_bJump = false;
			m_bDash = false;
			m_bAttack = false;
			m_bCharge = false;
		}
	}
}
