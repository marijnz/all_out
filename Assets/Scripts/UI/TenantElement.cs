using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TenantElement : MonoBehaviour
{
	public TextMeshProUGUI nameLabel;
	public TextMeshProUGUI trait1Label;
	public TextMeshProUGUI trait2Label;
	public Image image;
	public PointerEventHandler button;

	public void Init(GeneratedTenant tenant)
	{
		nameLabel.text = tenant.data.animalName;
		image.sprite = tenant.data.image;

		trait1Label.text = tenant.traits[0].name;
		trait2Label.text = tenant.traits[1].name;
	}
}