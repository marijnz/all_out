using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TenantElement : MonoBehaviour
{
	public TextMeshProUGUI nameLabel;
	public Image image;
	public PointerEventHandler button;

	public void Init(TenantData.TenantItem itemData)
	{
		nameLabel.text = itemData.name;
		image.sprite = itemData.image;
	}
}