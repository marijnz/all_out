using System.Collections.Generic;
using UnityEngine;

public class NavigationAgent : MonoBehaviour
{
	public Transform to;

	public float speed = 0.2f;

	List<Vector3> path = null;
	int pathIndex = 0;

	public bool IsMoving { get { return path != null; } }

	private SimulateTickRate _simulationTickRate = null;
	private Navigation _navigation = null;

	private void Start()
	{
		_simulationTickRate = FindObjectOfType<SimulateTickRate>();
		if (_simulationTickRate != null)
			_simulationTickRate.OnTickUpdate.AddListener(OnTickUpdate);
		_navigation = FindObjectOfType<Navigation>();
	}

	private void OnDestroy()
	{
		if (_simulationTickRate != null)
			_simulationTickRate.OnTickUpdate.RemoveListener(OnTickUpdate);
	}

	[ContextMenu("Move")]
	public void Move()
	{
		Move(to.position);
	}

	public void Move(Vector3 to)
	{
		path = _navigation.GetPath(transform.position, to);
	}

	void OnTickUpdate(int tick)
	{
		if (path != null && path.Count > 0)
		{
			var pathNodeDestination = path[pathIndex];

			var dist = Vector3.Distance(transform.position, pathNodeDestination);
			if (dist >.1f)
			{
				transform.position = Vector3.MoveTowards(transform.position, path[pathIndex], speed);
			}
			else
			{
				pathIndex++;
				if (pathIndex >= path.Count)
				{
					// Reached destination
					path = null;
					pathIndex = 0;
				}
			}
		}
	}
}