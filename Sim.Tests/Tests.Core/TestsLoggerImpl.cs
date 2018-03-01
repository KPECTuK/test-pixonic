using System;
using Sim.Module.Logger;

namespace Sim.Tests.Core {
	public class TestsLoggerImpl : LoggerImpl
	{
		public override void Log(Type source, Level level, object @object, Exception exception)
		{
			$"{source.Name}::[{level.DisplayName}] -> {@object} ({exception?.Message})".LogAux();
		}
	}
}