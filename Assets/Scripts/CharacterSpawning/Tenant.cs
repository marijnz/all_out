using System.Collections;
using UnityEngine;

public class Tenant : MonoBehaviour
{
	public SpriteRenderer spriteRenderer;

	public Sprite[] directionSprites;

	GeneratedTenant generatedTenant;

	IEnumerator Start()
	{
		while(true)
		{
			yield return new WaitForSeconds(4);
			Emotion.Show(this.transform , Random.value > .5f);
		}
	}

	Vector3 lastPos;

	public void SetData(GeneratedTenant generatedTenant)
	{
		this.generatedTenant = generatedTenant;
	}

	void Update()
	{
		var dir = transform.position - lastPos;

		if(directionSprites != null && directionSprites.Length > 0)
		{
			if(!Mathf.Approximately(Vector3.Distance(dir, Vector3.one), 0))
			{
				if(dir.x < 0 && dir.y > 0) spriteRenderer.sprite = directionSprites[0];
				if(dir.x > 0 && dir.y > 0) spriteRenderer.sprite = directionSprites[1];
				if(dir.x < 0 && dir.y < 0) spriteRenderer.sprite = directionSprites[2];
				if(dir.x > 0 && dir.y < 0) spriteRenderer.sprite = directionSprites[3];
			}
		}

		lastPos = transform.position;
	}
}