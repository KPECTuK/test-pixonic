using System;

namespace Sim.Module.Logger
{
	public interface ILogger
	{
		void Log(Type source, Level level, object @object, Exception exception);
	}
}