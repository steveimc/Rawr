using UnityEngine;
using System.Collections;

public class AnimationActions : MonoBehaviour 
{
	[SerializeField] HeroControllerSword m_HeroController;

	Collider2D hitZoneCollider;

	Collider2D spinZoneCollider;

	private void Start()
	{
		hitZoneCollider = transform.parent.GetComponentInChildren<HitZone>().gameObject.GetComponent<Collider2D>();
		spinZoneCollider = transform.parent.GetComponentInChildren<SpinZone>().gameObject.GetComponent<Collider2D>();
	}

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
		this.transform.rotation = Quaternion.Euler(0,180,0); 
		m_HeroController.m_animator.ChangeAnimator();
	}

	private void ToggleHitZone(int iStatus)
	{
		if(iStatus == 0)
		{
			hitZoneCollider.enabled = false;
		}
		else
		{
			hitZoneCollider.enabled = true;
			hitZoneCollider.gameObject.GetComponentInChildren<ParticleSystem>().Play();
		}
	}

	internal void ToggleSpinZone(int iStatus)
	{
		ParticleSystem[] particles;
		particles = spinZoneCollider.gameObject.GetComponentsInChildren<ParticleSystem>();
		if(iStatus == 0)
		{
			spinZoneCollider.enabled = false;

			foreach(ParticleSystem p in particles)
			{
				p.Stop();
			}
		
		}
		else
		{
			spinZoneCollider.enabled = true;
			foreach(ParticleSystem p in particles)
			{
				p.Play();
			}
		}
	}
}
