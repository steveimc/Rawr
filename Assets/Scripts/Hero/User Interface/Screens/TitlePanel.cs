using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitlePanel : MonoBehaviour 
{
	[SerializeField] private Button playButton;
	public void OnPlayButton()
	{
		float newX = transform.position.x - 1000;
		StartCoroutine(SlideHorizontal(transform.position.x, newX));
	}

	float slideSpeed = 8.0f;
	IEnumerator SlideHorizontal(float startX, float endX)
	{
		Vector3 newPos = transform.position;
		float t = 0;
		while (t < 1)
		{
			newPos.x = Mathf.Lerp(startX, endX, t * slideSpeed);
			this.transform.position = newPos;
			t += 0.01f;
			yield return new WaitForSeconds(0.01f);
		}
		playButton.gameObject.SetActive(false);
	}
}
