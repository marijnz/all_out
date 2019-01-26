using System.Collections.Generic;
using Entitas;

public sealed class ProcessTickSystem : ReactiveSystem<GameEntity>
{
	private GameContext _context = null;
	private SimulationFeature _feature = null;

	public ProcessTickSystem(GameContext context, SimulationFeature feature) : base(context)
	{
		_context = context;
		_feature = feature;
	}

	protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
	{
		return context.CreateCollector(GameMatcher.Tick);
	}

	protected override bool Filter(GameEntity entity)
	{
		return entity.hasTick;
	}

	protected override void Execute(List<GameEntity> entities)
	{
		GameEntity tickEntity = _context.tickEntity;
		if (!tickEntity.hasLastTick) tickEntity.ReplaceLastTick(0);

		for (int i = tickEntity.lastTick.Value; i < tickEntity.tick.Value; i++)
		{
			_feature.Execute();
		}
		
		tickEntity.ReplaceLastTick(tickEntity.tick.Value);
	}
}