namespace Sim.Module.Generic
{
	public interface ICommand<in T>
	{
		void Execute(T context);
	}
}