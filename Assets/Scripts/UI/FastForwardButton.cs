using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public sealed class FastForwardButton : MonoBehaviour, IPointerClickHandler
{
	private SimulateTickRate _simulationTickRate = null;
	[SerializeField] private TMP_Text _label = null;

	private void Start()
	{
		_simulationTickRate = FindObjectOfType<SimulateTickRate>();
		_simulationTickRate.OnTimeChanged += OnTimeChanged;
	}

	private void OnTimeChanged(int time)
	{
		_label.text = string.Format("{0}x", time);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		_simulationTickRate.IncreaseTimeMultiplier();
	}
}