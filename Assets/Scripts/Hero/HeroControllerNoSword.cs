using UnityEngine;
using System.Collections;

public class HeroControllerNoSword : HeroBaseController 
{
	private bool m_isOnWall;

	internal override void Move(float fHorizontal, float fVertical, bool bJump, bool bDash)
	{

		CheckDirection(fHorizontal);
		
		m_comboTimer += Time.fixedDeltaTime;

		if(m_comboTimer > 0.7f)
		{
			m_AttackType = 0;
			m_comboTimer = 0;
		}

		if(!IsGrounded() && !m_isFalling && !m_isOnWall)
		{
			animator.Fall();
			m_isFalling = true;
		}
		else if(IsGrounded())
		{
			animator.IsGrounded();
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

		if(IsWallded() && m_canWallJump)
		{
			if(!m_isOnWall)
			{
				m_isOnWall = true;
				animator.Wall();
			}

			m_timer += Time.fixedDeltaTime;

			if(m_timer < 2.0f)
			{
				m_HeroRigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
			}
			else
			{
				m_isOnWall = false;
				m_isFalling =false;
				m_HeroRigidBody.constraints =  RigidbodyConstraints2D.FreezeRotation;
				m_canWallJump = false;
				m_timer = 0;
			}

				m_isDashing = false;

			if(bJump)
			{
				//animator.WallJump();
				m_isOnWall = false;
				m_isFalling =false;
				bJump = false;
				m_isWallJumping = true;
				m_canWallJump = true;
				m_timer = 0;
				m_LastPos = this.transform.position;
				m_HeroRigidBody.constraints =  RigidbodyConstraints2D.FreezeRotation;

				if(m_Facing == Direction.RIGHT)
					m_HeroRigidBody.AddForce(new Vector2(-m_JumpForce*3, m_JumpForce));
				else
					m_HeroRigidBody.AddForce(new Vector2(m_JumpForce*3, m_JumpForce));
			}
		}
		else if(!IsWallded() && !m_isDashing && !m_isWallJumping )
		{
			if(m_isOnWall)
			{
				m_isOnWall = false;
				m_isFalling = false;
			}

			m_HeroRigidBody.constraints =  RigidbodyConstraints2D.FreezeRotation;
	
			if(IsMoving(fHorizontal) && !m_isCrouching && !m_isCharging)
			{
				// Move the character
				m_HeroRigidBody.velocity = new Vector2(Mathf.Clamp(m_HeroRigidBody.velocity.x + (fHorizontal/2) * m_MaxSpeed,-m_MaxSpeed,m_MaxSpeed), m_HeroRigidBody.velocity.y);
			}
			else
			{
				m_HeroRigidBody.velocity = new Vector2(Mathf.Clamp(m_HeroRigidBody.velocity.x * 0.9f,-m_MaxSpeed,m_MaxSpeed), m_HeroRigidBody.velocity.y);
			}

		}

		if(bDash && m_distance <= m_DashDistance && !m_isDashing && !m_isCharging)
		{
			bDash = false;
			m_isDashing = true;
			m_LastPos = this.transform.position;
			m_HeroRigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
			Dash();
		}
		else if(m_distance >= m_DashDistance)
		{
			m_isDashing = false;
			m_HeroRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
			m_HeroRigidBody.velocity = new Vector2(Mathf.Clamp(m_HeroRigidBody.velocity.x * 0.2f,-m_MaxSpeed,m_MaxSpeed), m_HeroRigidBody.velocity.y);
			m_distance = 0;
		}

		if(m_isDashing)
		{
			m_canWallJump = true;
			m_distance = Vector2.Distance(m_LastPos,this.transform.position);
			m_HeroRigidBody.velocity = new Vector2(Mathf.Clamp(m_HeroRigidBody.velocity.x,-m_MaxSpeed*3f,m_MaxSpeed*3f), m_HeroRigidBody.velocity.y);

		}

		if(m_isWallJumping && !m_isDashing)
		{
			m_isOnWall = false;
			m_distance = Vector2.Distance(m_LastPos,this.transform.position);
			m_HeroRigidBody.velocity = new Vector2(Mathf.Clamp(m_HeroRigidBody.velocity.x,-m_MaxSpeed,m_MaxSpeed), m_HeroRigidBody.velocity.y);
		}

		if(m_isWallJumping && m_distance >= m_DashDistance)
		{
			m_isWallJumping = false;
		}
	}

	internal override void Jump()
	{
		animator.Jump();

		m_timer = 0;
		m_canWallJump = true;
		m_HeroRigidBody.AddForce(new Vector2(0f, m_JumpForce));
	}
		
	internal override void Attack(float fHorizontal, bool bAttack, bool bCharge)
	{
		if(!bAttack)
			return;

		if(IsGrounded() && !m_isDashing)
			animator.Attack(m_AttackType);

	}

	internal override void CheckDirection(float fInput)
	{
		if(fInput < -0.2f)
		{
			m_Facing = Direction.LEFT;
			Flip (this.transform);
		}
		else if(fInput > 0.2f)
		{
			m_Facing = Direction.RIGHT;
			Flip (this.transform);
		}
	}

	internal override void Flip(Transform objectToFlip)
	{
		if(m_isCharging || m_isCrouching)
			return;
		
		float fScale = objectToFlip.localScale.y;

		if(m_Facing == Direction.RIGHT)
			objectToFlip.localScale = new Vector3(fScale,fScale,fScale);
		else
			objectToFlip.localScale = new Vector3(-fScale,fScale,fScale);
	}

	internal override void Dash()
	{
		m_isFalling = false;

		if(m_Facing == Direction.RIGHT)
			m_HeroRigidBody.AddForce(new Vector2(m_DashForce,0f));
		else
			m_HeroRigidBody.AddForce(new Vector2(-m_DashForce,0f));

		if(!m_isSpinning)
			animator.Dash();
	}
}
