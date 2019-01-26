using Entitas;
using UnityEngine;

public sealed class EmitStepInput : IExecuteSystem
{
	private InputContext _context = null;
	private GameConfig _config = null;
	private float _actualTick = 0.0f;

	public EmitStepInput(InputContext context, GameConfig config)
	{
		_context = context;
		_config = config;
	}

	public void Execute()
	{
		_actualTick += _config.TicksPerSecond * Time.deltaTime;
		if (_actualTick >= 1.0f)
		{
			int ticks = (int)_actualTick;
			_actualTick -= ticks;
			_context.CreateEntity().AddStepTick(ticks);
		}
	}
}