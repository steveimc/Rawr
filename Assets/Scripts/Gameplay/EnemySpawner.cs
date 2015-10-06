using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour 
{
	public struct Spawner
	{
		public int iStage;
		public int iMaxEnemies;
		public int iEnemyTypes;
	}

	public enum EnemyType
	{
		NONE,
		SPIRIT,
		SLUG,
		DRAGON

	}

	[SerializeField] EnemySpirit 	spirit;
	[SerializeField] EnemyDragon 	dragon;
	[SerializeField] EnemySlug 		slug;
	[SerializeField] Transform[] 	drSpawningPoints;
	[SerializeField] Transform[] 	slSpawningPoints;

	float interval = 0;
	int iSlugLastSpawningPoint = 0xffffff;


	public Spawner spawner = new Spawner();

	public void InitSpawner(int iStage, int iMaxEnemies, int iEnemyTypes)
	{
		spawner.iStage = iStage;
		spawner.iMaxEnemies = iMaxEnemies;
		spawner.iEnemyTypes = iEnemyTypes;
	}

	void Update()
	{
		interval += Time.deltaTime;
		if(GameManager.Instance.m_iEnemiesOnScreen < spawner.iMaxEnemies)
		{
			if(interval > Random.Range(1,5)) //TODO: Make random
			{
				SpawnEnemy();
				interval = 0;
			}
		}
	}

	void SpawnEnemy()
	{
		int type = Random.Range(1,spawner.iEnemyTypes+1);

		if(GameManager.Instance.m_iEnemiesOnScreen < spawner.iMaxEnemies)
		{
			int spawnIndex;

			switch(type)
			{
			case (int)EnemyType.SPIRIT:

				float height = 2f * Camera.main.orthographicSize;
				float width = height * Camera.main.aspect;

				Vector3 position = new Vector3(Random.Range(0,(int)width), Random.Range(0,(int)height), 0);

				Instantiate(spirit,position, Quaternion.identity);
				GameManager.Instance.m_iEnemiesOnScreen++;
				break;

			case (int)EnemyType.SLUG:
				spawnIndex = Random.Range(0,slSpawningPoints.Length);

				Instantiate(slug,slSpawningPoints[spawnIndex].position, slSpawningPoints[spawnIndex].rotation);
				GameManager.Instance.m_iEnemiesOnScreen++;
				break;

			case (int)EnemyType.DRAGON:

				if(FindObjectsOfType<EnemyDragon>().Length < 2)
				{
					spawnIndex = Random.Range(0,drSpawningPoints.Length);

					while(spawnIndex == iSlugLastSpawningPoint)
					{
						spawnIndex = Random.Range(0,drSpawningPoints.Length);
					}

					iSlugLastSpawningPoint = spawnIndex;

					Instantiate(dragon,drSpawningPoints[spawnIndex].position, drSpawningPoints[spawnIndex].rotation);
					GameManager.Instance.m_iEnemiesOnScreen++;
				}

				break;
			}
		}
		else
		{
			iSlugLastSpawningPoint = 0xffffff;
		}
	}
}
