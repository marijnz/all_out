using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class TenantGeneration : ScriptableObject
{
	[MenuItem("Assets/Create/TenantGeneration")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<TenantGeneration>();
	}

	public List<TenantTrait> traits;
	public List<string> names;
}

[Serializable]
public class TenantTrait
{
	public int id;
	public string name;
	public List<int> dislikes = new List<int>();
}