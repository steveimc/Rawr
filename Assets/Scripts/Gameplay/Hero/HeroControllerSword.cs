using UnityEngine;
using System.Collections;

public class HeroControllerSword : HeroBaseController 
{
	public Rigidbody2D m_FrostNova;
	internal bool m_isThrowingSword;

	internal override void Move(float fHorizontal, float fVertical, bool bJump, bool bDash, bool bJumpHold)
	{
		m_Facing = m_animator.CheckDirection();
		Flip(this.transform);

		/*
		m_comboTimer += Time.fixedDeltaTime;


		if(m_comboTimer > 0.7f)
		{
			m_AttackType = 0;
			m_comboTimer = 0;
		}
		*/

		if(!IsGrounded() && !m_isFalling)
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
			Jump();
		}
		else if(IsGrounded() && !m_isDashing)
		{
			Crouch(fVertical);
		}

		if(IsWallded() && m_canWallJump)
		{
			m_timer += Time.fixedDeltaTime;


			if(m_timer < 2.0f)
			{
				m_HeroRigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
			}
			else
			{
				m_HeroRigidBody.constraints =  RigidbodyConstraints2D.FreezeRotation;
				m_canWallJump = false;
				m_timer = 0;
			}

			m_isDashing = false;

			if(bJump)
			{
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
			m_HeroRigidBody.constraints =  RigidbodyConstraints2D.FreezeRotation;


			if(IsMoving(fHorizontal) && !m_isCrouching && !m_isCharging)
			{
				// Move the character
				m_HeroRigidBody.velocity = new Vector2(Mathf.Clamp(m_HeroRigidBody.velocity.x + (fHorizontal/2) * m_MaxSpeed,-m_MaxSpeed,m_MaxSpeed), m_HeroRigidBody.velocity.y);
			}
			else
			{
				m_HeroRigidBody.velocity = new Vector2(Mathf.Clamp(m_HeroRigidBody.velocity.x * 0.7f,-m_MaxSpeed,m_MaxSpeed), m_HeroRigidBody.velocity.y);
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
		m_animator.Jump();
	}
		
	internal override void Attack(float fHorizontal, bool bAttack, bool bCharge)
	{
		if(bCharge && !m_isSpinning && IsGrounded() && !IsMoving(fHorizontal) && m_AttackType == 0)
		{
			m_animator.Charge();
			m_isCharging = true;
			m_chargeTimer = 0;
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

		if(m_AttackType == 0)
			m_chargeTimer += Time.deltaTime;

		if(!bAttack)
			return;

		if(m_chargeTimer > 2 && !m_isSpinning && m_AttackType == 0 && m_isCharging)
		{
			this.GetComponentInChildren<AnimationActions>().ToggleSpinZone(1);
			m_animator.Spin();
			m_chargeTimer = 0;
			m_isSpinning = true;
			m_isCharging = false;
			AudioManager.instance.PlayFrom(GetComponent<AudioSource>(),Audio.Bank.SPIN_SWORD);
		}
		else
		{
			if(IsMoving(fHorizontal) && !m_isThrowingSword)
			{
				m_isThrowingSword = true;
				m_isCharging = false;
				ThrowSword();
			}
			else
			{
				m_isCharging = false;
				m_animator.Attack(m_AttackType);
				AudioManager.instance.PlayFrom(GetComponent<AudioSource>(),Audio.Bank.SWING_SWORD);

				/*
				if(m_AttackType == 0)
					m_comboTimer = 0;
				else if(m_AttackType > 2)
					m_AttackType = 0;
				*/
				//m_AttackType++;
			}
		}	

	}

	internal void ThrowSword()
	{
		m_animator.SpinToThrow();

	}
		
}
