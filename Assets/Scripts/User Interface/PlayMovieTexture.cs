using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayMovieTexture : MonoBehaviour 
{
	public void Start()
	{
		MovieTexture movie = (MovieTexture)this.GetComponent<RawImage>().mainTexture;
		movie.Play();
	}
}
