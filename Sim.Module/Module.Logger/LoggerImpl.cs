using System;

namespace Sim.Module.Logger
{
	public abstract class LoggerImpl : ILogger
	{
		public Type Owner { get; set; }
		public string OwnerName { get; set; }

		public abstract void Log(Type source, Level level, object @object, Exception exception);
	}
}