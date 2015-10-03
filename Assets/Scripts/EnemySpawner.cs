using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour 
{
	public struct Spawner
	{
		int iStage;
		int iMaxEnemies;
		int iEnemyTypes;
	}

	public Spawner spawner;

	// Update is called once per frame
	void Update () 
	{
	
	}

	void SpawnEnemy()
	{
		if(GameManager.Instance.spawner)
		{
		}
	}
}
