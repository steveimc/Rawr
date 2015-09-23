using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour 
{
	public enum AttackType
	{
		ATTACK_ONE,
		ATTACK_TWO,
		ATTACK_THREE
	}

	internal HeroStatus m_Hero;
	// Private ---------------------------------------

	private Animator animator;
	private Vector3 lastPosition;
	private Vector3 lastDirectionPosition;
	private Vector3 animatorInput;
	private float input;
	private float distance;

	private const float MULTIPLIER = 10f;
	private const float ANIMATOR_SMOOTHING = 0.1f;

	HeroBaseController.Direction direction = HeroBaseController.Direction.RIGHT;

	private void Awake()
	{
		m_Hero = GetComponent<HeroStatus>();
		lastPosition = transform.position;
		lastDirectionPosition = transform.position;
	}

	// Animate the character depending on it's movement and direction
	private void Update()
	{
		if(animator == null)
		{
			animator = GetComponentInChildren<Animator>();
			ChangeAnimator();
		}
		else
		{
			Vector3 heading = transform.position - lastPosition;
			lastPosition = transform.position;

			// Check the direction of the character and transform it from world space to local
			Vector3 direction = transform.InverseTransformDirection(heading);

			animatorInput = Vector3.Lerp(animatorInput, direction * MULTIPLIER, ANIMATOR_SMOOTHING);

			if(animatorInput.x < 0)
				input = animatorInput.x * -1;		
			else
				input = animatorInput.x;

			animator.SetFloat("VelX", input); // Set animator values
		}               
	}

	internal HeroBaseController.Direction CheckDirection()
	{
		Vector3 heading = transform.position - lastPosition;
		lastDirectionPosition = transform.position;

		// Check the direction of the character and transform it from world space to local
		Vector3 directionInput = transform.InverseTransformDirection(heading);

		if(directionInput.x < -0.1)
		{
			direction = HeroBaseController.Direction.LEFT;
		}
		else if(directionInput.x > 0.1)
		{
			direction = HeroBaseController.Direction.RIGHT;
		}

		return direction;
	}

	internal void ChangeAnimator()
	{
		if(animator == null)
			return;

		if(m_Hero.m_bHasSword)
			animator.runtimeAnimatorController = Resources.Load("Hero_Sword") as RuntimeAnimatorController;
		else
			animator.runtimeAnimatorController = Resources.Load("Hero_NoSword") as RuntimeAnimatorController;
	}

	internal void Dash()
	{
		animator.SetTrigger("Dash");
		m_Hero.SendInput(UserInput.Dash);
	}

	internal void Jump()
	{
		animator.SetTrigger("Jump");
		m_Hero.SendInput(UserInput.Jump);
	}

	internal void Crouch(float fCrouch)
	{
		animator.SetFloat("VelY", fCrouch);
	}

	internal void Attack(int iAttack)
	{
		switch((AttackType)iAttack)
		{
			case AttackType.ATTACK_ONE:
				animator.SetTrigger("Attack1");
				m_Hero.SendInput(UserInput.Attack1);
				break;

			case AttackType.ATTACK_TWO:
				animator.SetTrigger("Attack2");
				m_Hero.SendInput(UserInput.Attack2);
				break;

			case AttackType.ATTACK_THREE:
				animator.SetTrigger("Attack3");
				m_Hero.SendInput(UserInput.Attack3);
				break;
		}
	}

	internal void Charge()
	{
		animator.SetTrigger("Charge");
	}

	internal void Spin()
	{
		animator.SetTrigger("Spin");
	}

	internal void Fall()
	{
		animator.SetTrigger("Fall");
		animator.SetBool("IsGrounded", false);
	}

	internal void IsGrounded()
	{
		animator.SetBool("IsGrounded", true);
	}

	internal void Wall()
	{
		animator.SetTrigger("Wall");
	}

	internal void WallJump()
	{
		animator.SetTrigger("WallJump");
	}
}
