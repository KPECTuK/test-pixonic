using System;
using System.Collections.Generic;
using System.Reflection;
using log4net.Appender;
using log4net.Core;
using log4net.Util;
using Sim.Launcher.NetworkAppender.Core;
using Sim.Launcher.NetworkAppender.Filters;
using Sim.Launcher.NetworkAppender.Methods;

namespace Sim.Launcher.NetworkAppender
{
	public class NetworkAppender : AppenderSkeleton
	{
		private static readonly Type _declaringType = typeof(NetworkAppender);

		private IRepository<string> _repository;
		private INetworkMethod _method;

		// ReSharper disable MemberCanBePrivate.Global
		public string ConnectionString { get; set; }
		public Type FilterType { get; set; }
		public Type MethodType { get; set; }
		// ReSharper restore MemberCanBePrivate.Global

		protected override bool RequiresLayout => true;

		public override void ActivateOptions()
		{
			base.ActivateOptions();
			_repository = new MessageHeap<string>();
			try
			{
				var constructor =
					MethodType
						.GetConstructor(
							BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
							null,
							new[] { typeof(string), typeof(IEnumerator<string>), typeof(INetworkMethodFilter<string>) },
							null);
				_method =
					constructor
						.Invoke(
							new object[]
							{
								ConnectionString,
								_repository.GetEnumerator(),
								Activator.CreateInstance(FilterType) as INetworkMethodFilter<string> ?? new DefaultTitleFilter()
							}) as INetworkMethod;
			}
			catch(Exception exception)
			{
				LogLog.Error(_declaringType, "fall to initialize NetworkAppender: " + exception.GetType().Name);
			}
		}

		protected override void Append(LoggingEvent loggingEvent)
		{
			_repository.Enqueue(RenderLoggingEvent(loggingEvent));
			_method.Commit();
		}

		protected new void Close()
		{
			_method?.Dispose();
			base.Close();
		}
	}
}
