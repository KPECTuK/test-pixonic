using System;
using System.Reflection;

namespace Sim.Module.Logger
{
	public abstract class LoggerFactoryBase<TLogger> : ILoggerFactory where TLogger : LoggerImpl, new()
	{
		/// <inheritdoc />
		public ILogger GetOfName(string name)
		{
			return new TLogger { OwnerName = name };
		}

		/// <inheritdoc />
		public ILogger GetFor(Type type)
		{
			return new TLogger { Owner = type };
		}

		/// <inheritdoc />
		public abstract void RegisterRenderers(Assembly assembly);

		/// <inheritdoc />
		public ILogger Get()
		{
			return new TLogger();
		}
	}
}