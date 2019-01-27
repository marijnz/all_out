using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public sealed class IntEvent : UnityEvent<int> { }
public delegate void IntHandler(int i);

public sealed class SimulateTickRate : MonoBehaviour
{
	public event IntHandler OnTimeChanged;

	private bool _paused = false;
	[SerializeField] private int _ticksPerSecond = 60;
	[SerializeField] private IntEvent _onTickUpdate = null;
	public IntEvent OnTickUpdate { get { return _onTickUpdate; } }

	private float _actualTicks = 0.0f;
	private int _actualTickCount = 0;

	[SerializeField] private int[] _timeMultiplier = null;
	private int _actualMultiplierIndex = 0;

	private void Update()
	{
		if (_paused) return;

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

	public void IncreaseTimeMultiplier()
	{
		if (_timeMultiplier == null || _timeMultiplier.Length == 0) return;

		_actualMultiplierIndex = ++_actualMultiplierIndex % _timeMultiplier.Length;
		SetTime(_timeMultiplier[_actualMultiplierIndex]);
	}

	private void SetTime(int time)
	{
		Time.timeScale = time;

		OnTimeChanged?.Invoke(time);
	}

	public void Pause()
	{
		if (_paused) return;

		_paused = true;

		_actualMultiplierIndex = 0;
		if ((_timeMultiplier == null || _timeMultiplier.Length == 0))
			SetTime(1);
		else
			SetTime(_timeMultiplier[_actualMultiplierIndex]);
	}

	public void Unpause()
	{
		if (!_paused) return;
		_paused = false;
	}
}