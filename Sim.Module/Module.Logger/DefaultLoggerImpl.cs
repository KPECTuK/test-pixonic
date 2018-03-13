using System;
using UnityEngine;

namespace Sim.Module.Logger
{
	public class DefaultLoggerImpl : LoggerImpl
	{
		public override void Log(Type source, Level level, object @object, Exception exception)
		{
			Debug.Log($"{source.Name}::[{level.DisplayName}] -> {@object} ({exception?.Message})");
		}
	}
}