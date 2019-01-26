using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerEventHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
	const float AnimationDuration = 0.05f;

	public Action onClick;
	public Action onDown;
	public Action onUp;

	public bool interactable { get; set; }
	public TextMeshProUGUI label { get { return this.GetComponentInChildren<TextMeshProUGUI>(); } }

	public PointerEventHandler()
	{
		interactable = true;
	}

	public virtual void OnPointerDown(PointerEventData eventData)
	{
		if(!interactable) {  return; }

		onDown?.Invoke();

		if(gameObject.activeInHierarchy)
			StartCoroutine(DoScale(0.95f, AnimationDuration));
	}

	public virtual void OnPointerUp(PointerEventData eventData)
	{
		if (!interactable) { return; }

		onUp?.Invoke();

		if(gameObject.activeInHierarchy)
			StartCoroutine(DoScale(1, AnimationDuration));
	}

	public virtual void OnPointerClick(PointerEventData eventData)
	{
		if (!interactable) { return; }

		onClick?.Invoke();
	}

	void OnEnable()
	{
		transform.localScale = Vector3.one;
	}

	IEnumerator DoScale(float to, float duration)
	{
		float time = 0;
		var from = this.transform.localScale.x;

		while (time < duration)
		{
			var ease = Mathf.Lerp(from, to, time/duration);

			transform.localScale = Vector3.one * ease;

			yield return new WaitForEndOfFrame();
			time += Time.deltaTime;
		}
		this.transform.localScale = Vector3.one * to;
	}
}