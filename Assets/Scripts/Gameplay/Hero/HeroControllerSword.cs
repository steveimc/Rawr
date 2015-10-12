using UnityEngine;
using System.Collections;

public class HeroControllerSword : HeroBaseController 
{
	public Rigidbody2D m_FrostNova;
	internal bool m_isThrowingSword;

	internal override void Move(float fHorizontal, float fVertical, bool bJump, bool bDash, bool bJumpHold)
	{
		if(m_HeroRigidBody.velocity.y < -5)
			gameObject.layer = LayerMask.NameToLayer("Hero");

		m_Facing = m_animator.CheckDirection();
		Flip(this.transform);

		if(!IsGrounded() && m_HeroRigidBody.velocity.y < 0 && !m_isFalling)
		{
			m_animator.Fall();
			m_isFalling = true;
		}
		else if(IsGrounded())
		{
			m_animator.IsGrounded();
			m_isFalling = false;
		}

		if(bJump && IsGrounded() && !m_isDashing && !m_isCharging && !m_isCrouching)
		{
			bJump = false;
			//Jump();
		}
		else if(IsGrounded() && !m_isDashing)
		{
			Crouch(fVertical);
		}

		if(!m_isDashing && !m_isCrouching && !m_isAttacking && !m_isCharging)
		{
			if(IsMoving(fHorizontal) && !m_isCrouching && !m_isCharging)
			{
				if(IsGrounded())
					m_HeroRigidBody.velocity = new Vector2(Mathf.Clamp((fHorizontal * m_MaxSpeed),-m_MaxSpeed,m_MaxSpeed), m_HeroRigidBody.velocity.y);
				else
					m_HeroRigidBody.velocity = new Vector2(Mathf.Clamp(m_HeroRigidBody.velocity.x + (fHorizontal),-m_MaxSpeed,m_MaxSpeed), m_HeroRigidBody.velocity.y);
			}
			else if (!IsMoving(fHorizontal) && IsGrounded())
			{
				m_HeroRigidBody.velocity = new Vector2(Mathf.Clamp(m_HeroRigidBody.velocity.x * 0.8f,-m_MaxSpeed,m_MaxSpeed), m_HeroRigidBody.velocity.y);
			}
		}
		else if(!m_isDashing)
		{
			m_HeroRigidBody.velocity = new Vector2(Mathf.Clamp(m_HeroRigidBody.velocity.x * 0.8f,-m_MaxSpeed,m_MaxSpeed), m_HeroRigidBody.velocity.y);
		}


		if(bDash && m_distance <= m_DashDistance && !m_isDashing && !m_isCharging)
		{
			bDash = false;
			m_isDashing = true;
			m_LastPos = this.transform.position;
			m_HeroRigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
			Dash(fHorizontal);
		}
		else if(m_distance >= m_DashDistance || m_DashTimer > 0.5f)
		{
			m_UpperCollider.enabled = true;
			m_isDashing = false;
			m_HeroRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
			m_HeroRigidBody.velocity = new Vector2(Mathf.Clamp(m_HeroRigidBody.velocity.x * 0.8f,-m_MaxSpeed,m_MaxSpeed), m_HeroRigidBody.velocity.y);
			m_distance = 0;
			m_DashTimer = 0;
		}

		if(m_isDashing)
		{
			m_DashTimer += Time.deltaTime;
			m_distance = Vector2.Distance(m_LastPos,this.transform.position);
			m_HeroRigidBody.velocity = new Vector2(Mathf.Clamp(m_HeroRigidBody.velocity.x,-m_MaxSpeed*3f,m_MaxSpeed*3f), m_HeroRigidBody.velocity.y);
		}
	}

	internal override void Jump()
	{
		m_animator.Jump();
	}
		
	internal override void Attack(float fHorizontal, bool bAttack, bool bCharge)
	{

		if(bCharge && !m_isSpinning && IsGrounded() && !m_isAttacking && !m_isCharging )
		{
			m_animator.Charge(true);
			m_isCharging = true;
			m_chargeTimer = 0;
		}
		else if(m_chargeTimer > 2 && !m_isSpinning && m_isCharging)
		{
			this.GetComponentInChildren<AnimationActions>().ToggleSpinZone(1);
			m_animator.Spin();
			m_chargeTimer = 0;
			m_isSpinning = true;
			m_isCharging = false;
			AudioManager.instance.PlayFrom(GetComponent<AudioSource>(), Audio.Bank.SPIN_SWORD);
		}
		else if(!bCharge)
		{
			m_isCharging = false;
			m_animator.Charge(false);
		}

		if(m_isSpinning)
		{
			this.transform.Rotate(new Vector3(0,30,0));
			if(m_chargeTimer > 3)
			{
				this.GetComponentInChildren<AnimationActions>().ToggleSpinZone(0);
				m_isSpinning = false;
				this.transform.rotation = Quaternion.identity;
			}
		}

		m_chargeTimer += Time.deltaTime;

		if(!bAttack)
			return;
		/*
			if(IsMoving(fHorizontal) && !m_isThrowingSword)
			{
				m_isThrowingSword = true;
				m_isCharging = false;
				ThrowSword();
			}
			else */if(!m_isAttacking)
			{
				m_isCharging = false;
				m_isAttacking = true;
				m_animator.Attack(m_AttackType);
				AudioManager.instance.PlayFrom(GetComponent<AudioSource>(), Audio.Bank.SWING_SWORD);
			}
	}

	internal void ThrowSword()
	{
		m_animator.SpinToThrow();
	}
		
}
