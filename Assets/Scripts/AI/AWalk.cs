using UnityEngine;

[RequireComponent(typeof(NavigationAgent))]
public abstract class AWalk : MonoBehaviour
{
	protected NavigationAgent _navAgent = null;
	private SimulateTickRate _simulationTickRate = null;

	protected virtual void Awake()
	{
		_navAgent = GetComponent<NavigationAgent>();
	}

	protected virtual void Start()
	{
		_simulationTickRate = FindObjectOfType<SimulateTickRate>();
		if (_simulationTickRate != null)
			_simulationTickRate.OnTickUpdate.AddListener(OnTickUpdate);
	}

	protected virtual void OnDestroy()
	{
		if (_simulationTickRate != null)
			_simulationTickRate.OnTickUpdate.RemoveListener(OnTickUpdate);
	}

	protected abstract void OnTickUpdate(int tick);
}