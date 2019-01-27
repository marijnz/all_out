using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public sealed class IntEvent : UnityEvent<int> { }

public sealed class SimulateTickRate : MonoBehaviour
{
	public bool paused;
	[SerializeField] private int _ticksPerSecond = 60;
	[SerializeField] private IntEvent _onTickUpdate = null;
	public IntEvent OnTickUpdate { get { return _onTickUpdate; } }

	private float _actualTicks = 0.0f;
	private int _actualTickCount = 0;

	[SerializeField] private int[] _timeMultiplier = null;
	private int _actualMultiplierIndex = 0;

	private void Update()
	{
		if (paused) return;

		_actualTicks += _ticksPerSecond * Time.deltaTime;
		int ticks = (int) _actualTicks;

		if (ticks > 0)
		{
			_actualTicks -= ticks;
			for (int i = _actualTickCount; i < _actualTickCount + ticks; i++)
			{
				_onTickUpdate?.Invoke(i);
			}

			_actualTickCount += ticks;
		}
	}

	public int IncreaseTimeMultiplier()
	{
		if (_timeMultiplier == null || _timeMultiplier.Length == 0) return (int)Time.timeScale;

		_actualMultiplierIndex = ++_actualMultiplierIndex % _timeMultiplier.Length;
		Time.timeScale = _timeMultiplier[_actualMultiplierIndex];

		return _timeMultiplier[_actualMultiplierIndex];
	}
}