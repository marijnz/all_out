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
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			_label.text = string.Format("{0}x",
				_simulationTickRate.IncreaseTimeMultiplier());
			}
		}