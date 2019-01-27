using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TenantLeavingSentences : ScriptableObject
{
	[MenuItem("Assets/Create/TenantLeavingSentences")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<TenantLeavingSentences> ();
	}

	public List<string> texts = new List<string>();
}