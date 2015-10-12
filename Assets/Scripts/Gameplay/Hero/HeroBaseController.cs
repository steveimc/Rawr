using UnityEngine;
using System.Collections;

public abstract class HeroBaseController : MonoBehaviour 
{
	protected Rigidbody2D m_HeroRigidBody;

	internal AnimationController m_animator;

	internal Direction m_Facing;

	protected Vector3 m_LastPos;

	protected LayerMask m_GroundLayer;
	protected LayerMask m_WallLayer;

	protected int m_AttackType;

	[SerializeField] protected float m_MaxSpeed;                    // The fastest the player can travel in the x axis.
	[SerializeField] protected float m_JumpForce;                  // Amount of force added when the player jumps.
	[SerializeField] protected float m_DashForce;  
	[SerializeField] internal float m_ProjectileForce;
	[SerializeField] protected float m_DashDistance;
	protected float m_DashTimer;
	internal bool m_isAttacking;

	[SerializeField] protected BoxCollider2D m_UpperCollider;

	protected float m_timer;
	protected float m_comboTimer;
	protected float m_chargeTimer;
	protected float m_jumpTimer;
	protected float m_fAcceleration;

	protected float m_distance;

	protected bool m_isDashing;
	protected bool m_isWallJumping;
	protected bool m_isCrouching;
	internal bool m_isSpinning;
	internal bool m_isCharging;
	protected bool m_isFalling;
	protected bool m_isJumping;

	protected bool m_canWallJump;

	public enum Direction
	{
		NONE,
		RIGHT,
		LEFT
	}

	protected const float CIRCLE_DIAMETER 	= 1.5f;
	protected const float RAY_DISTANCE 		= 1.0f;

	public void Awake()
	{
		m_GroundLayer 	= 1 << LayerMask.NameToLayer("Ground");
		m_WallLayer 	= 1 << LayerMask.NameToLayer("Wall");
		m_distance 		= 0.0f;
		m_DashDistance 	= 5.0f;
		m_animator 		= GetComponent<AnimationController>();
		m_HeroRigidBody = GetComponent<Rigidbody2D>();
		m_canWallJump 	= true;
	}

	internal virtual void ResetAttackType()
	{
		m_AttackType = 0;
	}

	internal virtual bool IsMoving(float fHorizontal)
	{
		return (fHorizontal > 0.1f || fHorizontal < -0.1f);
	}

	internal virtual bool IsGrounded()
	{
		bool bGrounded = false;

		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, CIRCLE_DIAMETER, m_GroundLayer.value);

		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
				bGrounded = true;
		}

		return bGrounded;
	}

	internal virtual bool IsWallded()
	{
		RaycastHit2D hit;
		bool bWallded = false;

		if(m_Facing == Direction.RIGHT)
			hit = Physics2D.Raycast(transform.position, Vector2.right, RAY_DISTANCE,m_WallLayer.value);
		else
			hit = Physics2D.Raycast(transform.position, Vector2.left, RAY_DISTANCE,m_WallLayer.value);

		if(hit.collider != null)
		{
			bWallded = true;
		}
		
		return bWallded;
	}

	internal virtual void Crouch(float fVertical)
	{
		m_animator.Crouch(fVertical);

		if(fVertical < -0.3 && m_isCrouching == false)
		{
			if(m_animator.m_Hero.syncInput != null)
				m_animator.m_Hero.syncInput.SendInput(UserInput.Crouch);

			m_isCrouching = true;
			m_UpperCollider.enabled = false;
		}
		else if(fVertical > -0.3 && m_isCrouching == true)
		{
			if(m_animator.m_Hero.syncInput != null)
				m_animator.m_Hero.syncInput.SendInput(UserInput.CrouchStop);

			m_isCrouching = false;
			m_UpperCollider.enabled = true;
		}
	}

	internal virtual void Flip(Transform objectToFlip)
	{
		if(m_isCharging || m_isCrouching)
			return;

		float fScale = objectToFlip.localScale.y;

		if(m_Facing == Direction.RIGHT)
			objectToFlip.localScale = new Vector3(fScale,fScale,fScale);
		else if(m_Facing == Direction.LEFT)
			objectToFlip.localScale = new Vector3(-fScale,fScale,fScale);

	}

	internal virtual void Dash(float fHorizontal)
	{
		m_isFalling = false;

		if(fHorizontal < -0.1)
			m_HeroRigidBody.velocity = new Vector2(-m_DashForce,0f);
		else if(fHorizontal > 0.1)
			m_HeroRigidBody.velocity = new Vector2(m_DashForce,0f);
		else
		{
			if(m_Facing == Direction.RIGHT)
				m_HeroRigidBody.velocity = new Vector2(m_DashForce,0f);
			else if(m_Facing == Direction.LEFT)
				m_HeroRigidBody.velocity = new Vector2(-m_DashForce,0f);
		}

		if(!m_isSpinning)
		{
			m_animator.Dash();
			m_UpperCollider.enabled = false;
		}
	}


	internal abstract void Move(float fHorizontal, float fVertical, bool bJump, bool bDash, bool bJumpHold);

	internal abstract void Jump();

	internal abstract void Attack(float fHorizontal, bool bAttack, bool bCharge);

}
