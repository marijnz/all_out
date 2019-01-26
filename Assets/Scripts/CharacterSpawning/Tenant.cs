using System.Collections;
using UnityEngine;

public class Tenant : MonoBehaviour
{
	public SpriteRenderer spriteRenderer;

	IEnumerator Start()
	{
		while(true)
		{
			yield return new WaitForSeconds(4);
			Emotion.Show(this.transform , Random.value > .5f);
		}
	}
}