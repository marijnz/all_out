using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NavigationAgent))]
public class Tenant : MonoBehaviour
{
	public SpriteRenderer spriteRenderer;

	public Sprite[] directionSprites;

	GeneratedTenant generatedTenant;
	public List<TenantTrait> Traits { get { return generatedTenant.traits; } }

	Vector3 lastPos;

	private TenantEvaluator _tenantEvaluator = null;
	private NavigationAgent _navigationAgent = null;

	public bool IsMoving { get { return _navigationAgent.IsMoving; } }

	private void Start()
	{
		_tenantEvaluator = FindObjectOfType<TenantEvaluator>();
		_tenantEvaluator.Add(this);
		_navigationAgent = GetComponent<NavigationAgent>();
	}

	private void OnDestroy()
	{
		_tenantEvaluator.Remove(this);
	}

	public void SetData(GeneratedTenant generatedTenant)
	{
		this.generatedTenant = generatedTenant;
	}

	public void StopMoving()
	{
		_navigationAgent.Stop();
	}

	void Update()
	{
		var dir = transform.position - lastPos;

		if (directionSprites != null && directionSprites.Length > 0)
		{
			if (!Mathf.Approximately(Vector3.Distance(dir, Vector3.one), 0))
			{
				if (dir.x < 0 && dir.y > 0) spriteRenderer.sprite = directionSprites[0];
				if (dir.x > 0 && dir.y > 0) spriteRenderer.sprite = directionSprites[1];
				if (dir.x < 0 && dir.y < 0) spriteRenderer.sprite = directionSprites[2];
				if (dir.x > 0 && dir.y < 0) spriteRenderer.sprite = directionSprites[3];
			}
		}

		lastPos = transform.position;
	}
}