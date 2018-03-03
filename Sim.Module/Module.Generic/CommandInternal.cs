namespace Sim.Module.Generic
{
	public abstract class CommandInternal : ICommand, IContextInjector
	{
		public IContext Context { protected get; set; }
		public abstract void Execute();
	}
}