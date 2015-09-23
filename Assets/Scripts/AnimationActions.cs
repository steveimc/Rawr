using UnityEngine;
using System.Collections;

public class AnimationActions : MonoBehaviour 
{
	[SerializeField] HeroControllerSword m_HeroController;

	private void ThrowSword()
	{
		m_HeroController.m_isThrowingSword = false;
		m_HeroController.m_animator.m_Hero.ToggleSword();
		m_HeroController.m_animator.m_Hero.ChangeHeroController();

		Rigidbody2D projectile;
		projectile = Instantiate(m_HeroController.m_FrostNova, m_HeroController.transform.position, m_HeroController.transform.rotation) as Rigidbody2D;

		if(m_HeroController.m_Facing == HeroBaseController.Direction.RIGHT)
		{
			m_HeroController.Flip (projectile.transform);
			projectile.velocity = m_HeroController.transform.TransformDirection(Vector2.right * m_HeroController.m_ProjectileForce);
		}
		else
		{
			m_HeroController.Flip (projectile.transform);
			projectile.velocity = m_HeroController.transform.TransformDirection(Vector2.left * m_HeroController.m_ProjectileForce);
		}

		GameManager.Instance.m_FrostNovaCopy = projectile.gameObject;
	}

	private void ChangeAnimator()
	{
		m_HeroController.m_animator.ChangeAnimator();
	}
}
