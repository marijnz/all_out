using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public sealed class IntEvent : UnityEvent<int> { }

public sealed class SimulateTickRate : MonoBehaviour
{
	[SerializeField] private int _ticksPerSecond = 10;
	[SerializeField] private IntEvent _onTickUpdate = null;
	public IntEvent OnTickUpdate { get { return _onTickUpdate; } }

	private float _actualTicks = 0.0f;
	private int _actualTickCount = 0;

	private void Update()
	{
		_actualTicks += _ticksPerSecond * Time.deltaTime;
		int ticks = (int) _actualTicks;

		if (ticks > 0)
		{
			_actualTicks -= ticks;
			for (int i = _actualTickCount; i < _actualTickCount + ticks; i++)
			{
				Debug.Log("tick");
				_onTickUpdate?.Invoke(i);
			}

			_actualTickCount += ticks;
		}
	}
}