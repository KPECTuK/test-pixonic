namespace Sim.Module.Data
{
	public interface IDataContainer<out T> where T : IDataContainer<T>, new()
	{
		T Copy();
	}
}