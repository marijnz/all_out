using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class TenantData : ScriptableObject
{
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