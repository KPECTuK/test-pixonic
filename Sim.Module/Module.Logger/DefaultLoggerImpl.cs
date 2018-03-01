using System;

namespace Sim.Module.Logger {
	public class DefaultLoggerImpl : LoggerImpl
	{
		public override void Log(Type source, Level level, object @object, Exception exception)
		{
			Console.WriteLine($"{source.Name}::[{level.DisplayName}] -> {@object} ({exception?.Message})");
		}
	}
}