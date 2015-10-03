using UnityEngine;
using System.Collections;

public class AudioManager : Singleton<AudioManager> 
{
	public void Awake()
	{
		Audio.Bank = GetComponent<AudioBank>();
		DontDestroyOnLoad(this);
	}

	public void PlayWorld(AudioClip audioClip)
	{
		AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
	}

	public void Play(AudioSource audioSource, AudioClip audioClip)
	{
		audioSource.clip = audioClip;
		audioSource.Play();
	}

	public void Stop(AudioSource audioSource)
	{
		audioSource.Stop();
	}

	public void Loop(AudioSource audioSource, bool bStatus)
	{
		audioSource.loop = bStatus;
	}
}

public struct Audio
{
	public static AudioBank Bank;
}


