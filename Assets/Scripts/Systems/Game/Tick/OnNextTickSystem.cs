using System.Collections.Generic;
using Entitas;

public sealed class OnNextTickSystem : ReactiveSystem<InputEntity>
{
	private InputContext _context = null;
	private GameContext _gameContext = null;

	public OnNextTickSystem(InputContext context, GameContext gameContext) : base(context)
	{
		_context = context;
		_gameContext = gameContext;
	}

	protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
	{
		return context.CreateCollector(InputMatcher.StepTick);
	}

	protected override bool Filter(InputEntity entity)
	{
		return entity.hasStepTick;
	}

	protected override void Execute(List<InputEntity> entities)
	{
		foreach (InputEntity entity in entities)
		{
			_gameContext.ReplaceTick(_gameContext.tick.Value + entity.stepTick.Value);
		}
	}
}