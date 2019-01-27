using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public sealed class RandomWalk : AWalk
{
	private TileGrid _tileGrid = null;
	private Vector3[] _tilePositions = null;

	[SerializeField] private int _ticksToWait = 0;
	private int _nextAttempt = 0;

	override protected void Start()
	{
		base.Start();

		_tileGrid = FindObjectOfType<TileGrid>();

		List<Vector3> positions = new List<Vector3>();
		foreach (Vector3Int position in _tileGrid.walkableTileMap.cellBounds.allPositionsWithin)
		{
			if (_tileGrid.walkableTileMap.GetTile(position) == null) continue;
			if (_tileGrid.nonWalkableTileMap.GetTile(position) != null) continue;

			positions.Add(_tileGrid.walkableTileMap.CellToWorld(position));
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