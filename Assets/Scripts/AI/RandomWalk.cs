using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public sealed class RandomWalk : AWalk
{
	[SerializeField] private Tilemap _tilemap = null;
	private Vector3[] _tilePositions = null;

	[SerializeField] private int _ticksToWait = 0;
	private int _nextAttempt = 0;

	override protected void Start()
	{
		base.Start();

		List<Vector3> positions = new List<Vector3>();
		foreach (Vector3Int position in _tilemap.cellBounds.allPositionsWithin)
		{
			if (_tilemap.GetTile(position) == null) continue;

			positions.Add(_tilemap.CellToWorld(position));
		}

		_tilePositions = positions.ToArray();
	}

	protected override void OnTickUpdate(int tick)
	{
		if (_navAgent.IsMoving)
		{
			_nextAttempt = tick + _ticksToWait;
			return;
		}

		if (tick < _nextAttempt) return;

		_navAgent.Move(_tilePositions[Random.Range(0, _tilePositions.Length)]);
	}
}