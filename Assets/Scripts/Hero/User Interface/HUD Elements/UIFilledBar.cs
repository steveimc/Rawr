using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent (typeof (Image))]
public class UIFilledBar : MonoBehaviour 
{
	[SerializeField] private Image bar;
	[SerializeField] private Image lagBar;
	private int maxValue;
	private int currentValue;

	public void Init(int maxValue, int currentValue)
	{
		this.maxValue = maxValue;
		this.currentValue = currentValue;
	}

	public void UpdateValue(int newValue)
	{
		if(newValue == currentValue)
			return;

		currentValue = newValue;
		UpdateView();
	}

	void UpdateView()
	{
		float lagFill = bar.fillAmount;
		if(currentValue <= 0)
		{
			currentValue = 0;
			bar.fillAmount = 0;
		}
		else
		{
			bar.fillAmount = (float)currentValue / (float)maxValue;
		}
		
		StartCoroutine(DoLabelLag(lagFill, bar.fillAmount));
	}

	IEnumerator DoLabelLag(float startFill, float endFill)
	{
		float t = 0;
		while (t <= 1)
		{
			lagBar.fillAmount = Mathf.Lerp(startFill, endFill, t);
			t += 0.2f;
			yield return new WaitForSeconds(0.06f);
		}
	}
}
