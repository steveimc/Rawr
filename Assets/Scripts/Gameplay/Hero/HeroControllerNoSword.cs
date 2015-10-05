using UnityEngine;
using System.Collections;

public class HeroControllerNoSword : HeroBaseController 
{
	private bool m_isOnWall;

	internal override void Move(float fHorizontal, float fVertical, bool bJump, bool bDash, bool bJumpHold)
	{
		if(m_HeroRigidBody.velocity.y < -15)
			gameObject.layer = LayerMask.NameToLayer("Hero");
		
		m_Facing = m_animator.CheckDirection();
		Flip(this.transform);

		if(!IsGrounded() && !m_isFalling && !m_isOnWall)
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

		if(!IsGrounded() && IsWallded() && m_canWallJump)
		{

			if(!m_isOnWall)
			{
				m_isOnWall = true;
				m_animator.Wall();
			}

			m_timer += Time.fixedDeltaTime;

			m_isDashing = false;

			if(bJump)
			{
				gameObject.layer = LayerMask.NameToLayer("HeroJump");

				if(m_Facing == Direction.RIGHT)
					m_HeroRigidBody.velocity = new Vector2(-15, 16);
				else
					m_HeroRigidBody.velocity = new Vector2(15, 16);

				//animator.WallJump();
				m_isOnWall = false;
				m_isFalling =false;
				m_isJumping = true;
				bJump = false;
				m_isWallJumping = true;
				m_canWallJump = true;
				m_timer = 0;
				m_LastPos = this.transform.position;
				return;
			}

			if(m_timer < 2.0f)
			{
				m_HeroRigidBody.velocity = new Vector2(m_HeroRigidBody.velocity.x , m_HeroRigidBody.velocity.y * 0.5f);
			}
			else
			{
				m_isOnWall = false;
				m_isFalling =false;
				m_canWallJump = false;
				m_timer = 0;
			}
				


		}
		else if(!m_isDashing && !m_isWallJumping )
		{
			if(m_isOnWall)
			{
				m_isOnWall = false;
				m_isFalling = false;
			}
	
			if(IsMoving(fHorizontal) && !m_isCrouching && !m_isCharging)
			{
				m_fAcceleration += Time.deltaTime * 2.5f;
				m_fAcceleration = Mathf.Clamp(m_fAcceleration,0f,1f);
				// Move the character
				if(IsGrounded())
					m_HeroRigidBody.velocity = new Vector2(Mathf.Clamp((fHorizontal * m_MaxSpeed) * m_fAcceleration,-m_MaxSpeed,m_MaxSpeed), m_HeroRigidBody.velocity.y);
				else
					m_HeroRigidBody.velocity = new Vector2(Mathf.Clamp(m_HeroRigidBody.velocity.x + (fHorizontal),-m_MaxSpeed,m_MaxSpeed), m_HeroRigidBody.velocity.y);
			}
			else if (!IsMoving(fHorizontal) && IsGrounded())
			{
				m_fAcceleration = 0;
				m_HeroRigidBody.velocity = new Vector2(Mathf.Clamp(m_HeroRigidBody.velocity.x * 0.8f,-m_MaxSpeed,m_MaxSpeed), m_HeroRigidBody.velocity.y);
			}

		}

		if(bDash && m_distance <= m_DashDistance && !m_isDashing && !m_isCharging)
		{
			bDash = false;
			m_isDashing = true;
			m_LastPos = this.transform.position;
			m_HeroRigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
			Dash(fHorizontal);
		}
		else if(m_distance >= m_DashDistance)
		{
			m_UpperCollider.enabled = true;
			m_isDashing = false;
			m_HeroRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
			m_HeroRigidBody.velocity = new Vector2(Mathf.Clamp(m_HeroRigidBody.velocity.x * 0.8f,-m_MaxSpeed,m_MaxSpeed), m_HeroRigidBody.velocity.y);
			m_distance = 0;
		}

		if(m_isDashing)
		{
			m_canWallJump = true;
			m_distance = Vector2.Distance(m_LastPos,this.transform.position);
			//m_HeroRigidBody.velocity = new Vector2(Mathf.Clamp(m_HeroRigidBody.velocity.x,-m_MaxSpeed*3f,m_MaxSpeed*3f), m_HeroRigidBody.velocity.y);

		}

		if(m_isWallJumping && !m_isDashing)
		{
			m_isOnWall = false;
			m_distance = Vector2.Distance(m_LastPos,this.transform.position);
			//m_HeroRigidBody.velocity = new Vector2(Mathf.Clamp(m_HeroRigidBody.velocity.x,-m_MaxSpeed,m_MaxSpeed), m_HeroRigidBody.velocity.y);
		}

		if(m_isWallJumping && m_distance >= m_DashDistance)
		{
			m_isWallJumping = false;
		}

		if(m_isJumping)
		{
			m_jumpTimer += Time.deltaTime;

			if(bJumpHold)
			{
				m_HeroRigidBody.velocity = new Vector2(m_HeroRigidBody.velocity.x, m_HeroRigidBody.velocity.y + (m_jumpTimer * 2));
			}

			if(m_jumpTimer > 0.5f)
			{
				m_isJumping = false;
				m_jumpTimer = 0;
			}
		}

	}

	internal override void Jump()
	{
		gameObject.layer = LayerMask.NameToLayer("HeroJump");

		m_animator.Jump();

		m_timer = 0;
		m_canWallJump = true;
		m_isJumping = true;
		m_HeroRigidBody.velocity = new Vector2(m_HeroRigidBody.velocity.x,15);
	}
		
	internal override void Attack(float fHorizontal, bool bAttack, bool bCharge)
	{
		if(!bAttack)
			return;

		if(IsGrounded() && !m_isDashing)
			m_animator.Attack(m_AttackType);

	}
		
}
