using System;
using System.Reflection;
using Sim.Module.Generic;

namespace Sim.Module.Logger
{
	public interface ILoggerFactory : IProvider<ILogger>
	{
		/// <summary>
		///     Возвращает логгер по имени в формате log4net.
		/// </summary>
		ILogger GetOfName(string name);

		/// <summary>
		///     Возвращает логгер по типу.
		/// </summary>
		ILogger GetFor(Type type);

		/// <summary>
		///     Регистрирует рендереры обьектов в логере, должен вызываться только один раз.
		/// </summary>
		void RegisterRenderers(Assembly assembly);
	}
}