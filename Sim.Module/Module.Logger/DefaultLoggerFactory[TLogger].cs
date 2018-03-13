using System.Reflection;

namespace Sim.Module.Logger
{
	public class DefaultLoggerFactory<TLogger> : LoggerFactoryBase<TLogger> where TLogger : LoggerImpl, new()
	{
		public override void RegisterRenderers(Assembly assembly) { }
	}
}