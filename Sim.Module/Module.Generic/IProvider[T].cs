namespace Sim.Module.Generic
{
	public interface IProvider<T>
	{
		T Get();
	}
}