using UnityEngine;
using System.Collections;

public class AudioManager : Singleton<AudioManager> 
{
	AudioSource audioSource;
	public void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		Audio.Bank = GetComponent<AudioBank>();
		DontDestroyOnLoad(this);
	}

	public void PlayWorld(AudioClip audioClip)
	{
		AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
	}

	public void Play(AudioClip audioClip)
	{
		audioSource.clip = audioClip;
		audioSource.Play();
	}

	public void PlayFrom(AudioSource myAudioSource, AudioClip audioClip)
	{
		myAudioSource.clip = audioClip;
		myAudioSource.Play();
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


