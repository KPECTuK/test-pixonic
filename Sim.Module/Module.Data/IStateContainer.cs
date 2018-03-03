namespace Sim.Module.Data
{
	public interface IStateContainer<T> : IDataContainer<T> where T : IStateContainer<T>, new()
	{
		T GetDifference(T source);
		void ApplyDifference(T difference);
	}
}