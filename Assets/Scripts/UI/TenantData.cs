using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class TenantData : ScriptableObject
{
	[MenuItem("Assets/Create/TenantData")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<TenantData> ();
	}

	public List<TenantItem> potentialTenants = new List<TenantItem>();

	[Serializable]
	public class TenantItem
	{
		public string animalName;
		public Sprite previewImage;
		public Sprite image;
		public Sprite[] directionImages;
		public AudioClip audioClip;
	}
}