using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game]
public sealed class LastTickComponent : IComponent
{
	public int Value;
}