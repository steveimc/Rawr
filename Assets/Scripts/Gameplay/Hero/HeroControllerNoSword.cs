using UnityEngine;
using System.Collections;

public class HeroControllerNoSword : HeroBaseController 
{
	private bool m_isOnWall;

	internal override void Move(float fHorizontal, float fVertical, bool bJump, bool bDash, bool bJumpHold)
	{
		if(m_HeroRigidBody.velocity.y < -10)
			gameObject.layer = LayerMask.NameToLayer("Hero");
		
		m_Facing = m_animator.CheckDirection();
		Flip(this.transform);

		if(!IsGrounded() && m_HeroRigidBody.velocity.y < 0 && !m_isFalling)
		{
			m_animator.Fall();
			m_isFalling = true;
		}
		else if(IsGrounded() && gameObject.layer == LayerMask.NameToLayer("Hero"))
		{
			m_animator.IsGrounded();
			m_isFalling = false;
			m_isOnWall = false;
		}

		if(bJump && IsGrounded() && !m_isDashing && !m_isCharging && !m_isCrouching)
		{
			bJump = false;
			Jump();
		}
		else if(IsGrounded() && !m_isDashing)
		{
			Crouch(fVertical);
		}

		if(!m_isDashing && !m_isCrouching && !m_isAttacking)
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
			m_DashTimer  = 0;
		}

		if(m_isDashing)
		{
			m_DashTimer += Time.deltaTime;
			m_canWallJump = true;
			m_distance = Vector2.Distance(m_LastPos,this.transform.position);
		}
	}

	internal override void Jump()
	{
		gameObject.layer = LayerMask.NameToLayer("HeroJump");

		m_animator.Jump();

		AudioManager.instance.PlayFrom(GetComponent<AudioSource>(), Audio.Bank.JUMP);

		m_timer = 0;
		m_canWallJump = true;
		m_isJumping = true;
		m_HeroRigidBody.velocity = new Vector2(m_HeroRigidBody.velocity.x,	35);
	}
		
	internal override void Attack(float fHorizontal, bool bAttack, bool bCharge)
	{
		if(!bAttack)
			return;

		if(IsGrounded() && !m_isDashing)
			m_animator.Attack(m_AttackType);

	}
		
}
