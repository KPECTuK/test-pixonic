using System;
using System.Reflection;
using log4net.Core;
using Sim.Module.Logger;

namespace Sim.Launcher
{
	public class LauncherLoggerImpl : LoggerImpl
	{
		private log4net.Core.ILogger _logger;

		public override void Log(Type source, Module.Logger.Level level, object @object, Exception exception)
		{
			_logger = _logger ?? LoggerManager.GetLogger(Assembly.GetExecutingAssembly(), Owner);
			// level - slow
			_logger.Log(Owner, new log4net.Core.Level(level.Value, level.Name), @object, exception);
		}
	}
}