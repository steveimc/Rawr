using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour 
{
	public string levelName;
	AsyncOperation async;
	public UIFilledBar progress;
	
	public void Start() 
	{
		progress.Init(100, 0);
		progress.UpdateValue(0);
		StartCoroutine(loadLevel());
	}

	IEnumerator loadLevel()
	{
		yield return new WaitForSeconds(2.0f);
		async = Application.LoadLevelAsync(levelName);
		async.allowSceneActivation = false;
		while (!async.isDone)
		{
			//int progress=(int)((float)((float)maxWidth/100)*((int)(async.progress*100)));
			//Debug.Log(async.progress);
			progress.UpdateValue((int)(async.progress * 100) );
			if( async.progress >= 0.9f)
			{
				Debug.Log("here");
				progress.UpdateValue(100);
				yield return new WaitForSeconds(2.0f);
				async.allowSceneActivation = true;
			}
			yield return(0);
		}
	}
}
