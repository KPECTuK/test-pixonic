using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using Sim.Module.Extensions;
using Sim.Module.Logger;

namespace Sim.Module.Generic
{
	public class ServiceLocator : IContext
	{
		private readonly Type _selfType;
		private readonly IContext _base;

		private ILogger _logger;
		private ListDictionary _singletons;
		private ListDictionary _providers;

		public ServiceLocator()
		{
			_selfType = GetType();
		}

		public ServiceLocator(IContext @base) : this()
		{
			_base = @base;
		}

		public void RegisterInstance(object instance)
		{
			_singletons = _singletons ?? new ListDictionary();
			// instances are initializing externally and have no container events responders
			var type = instance.GetType();
			if(_singletons.Contains(type))
			{
				throw new ArgumentException(
					_singletons
						.ToText(
							$"registered already as {type.NameNice()}",
							_ =>
							{
								var key = (((DictionaryEntry)_).Key as Type).NameNice();
								var value = ((DictionaryEntry)_).Value.GetType().NameNice();
								return $"{key}\t\t -> {value}";
							}));
			}

			_singletons.Add(type, instance);
		}

		public void RegisterInstance<T>(T instance)
		{
			_singletons = _singletons ?? new ListDictionary();
			// instances are initializing externally and have no container events responders
			var type = typeof(T);
			if(_singletons.Contains(type))
			{
				throw new ArgumentException(
					_singletons
						.ToText(
							$"registered already as {type.NameNice()}",
							_ =>
							{
								var key = (((DictionaryEntry)_).Key as Type).NameNice();
								var value = ((DictionaryEntry)_).Value.GetType().NameNice();
								return $"{key}\t\t -> {value}";
							}));
			}

			_singletons.Add(type, instance);
		}

		public void RegisterProvider<T>(IProvider<T> provider)
		{
			_providers = _providers ?? new ListDictionary();
			var type = typeof(T);
			if(_providers.Contains(type))
			{
				throw new ArgumentException(
					_singletons
						.ToText(
							$"registered already as {type.NameNice()}",
							_ =>
							{
								var key = (((DictionaryEntry)_).Key as Type).NameNice();
								var value = ((DictionaryEntry)_).Value.GetType().NameNice();
								return $"{key}\t\t -> {value}";
							}));
			}

			_providers.Add(type, provider);
		}

		public object ResolveSingletonOrType(Type type)
		{
			if(_singletons?.Contains(type) ?? false)
			{
				return _singletons[type];
			}

			object instance;
			try
			{
				var ctor = type
					.GetConstructor(
						BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
						null,
						new[] { typeof(IContext) },
						null);
				instance = ctor == null
					? Activator.CreateInstance(type)
					: ctor.Invoke(new object[] { this });
			}
			catch(Exception exception)
			{
				if(type != typeof(ILoggerFactory))
				{
					(_logger ?? (_logger = Resolve<ILoggerFactory>()?.GetFor(_selfType)))?.Log(_selfType, Level.Warn, $"can't build type of: {type.NameNice()}", exception);
				}

				return null;
			}

			return instance;
		}

		public T Resolve<T>() where T : class
		{
			T instance;
			return TryResolve(out instance)
				? instance
				: null;
		}

		public bool TryResolve<T>(out T instance) where T : class
		{
			if(_singletons?.Contains(typeof(T)) ?? false)
			{
				instance = _singletons[typeof(T)] as T;
				return true;
			}

			if(_providers?.Contains(typeof(T)) ?? false)
			{
				instance = (_providers[typeof(T)] as IProvider<T>)?.Get();
				return true;
			}

			instance = null;
			if(_base?.TryResolve(out instance) ?? false)
			{
				return true;
			}

			try
			{
				var ctor = typeof(T)
					.GetConstructor(
						BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
						null,
						new[] { typeof(IContext) },
						null);
				instance = ctor == null
					? Activator.CreateInstance<T>()
					: ctor.Invoke(new object[] { this }) as T;
				return true;
			}
			catch(Exception exception)
			{
				if(typeof(T) != typeof(ILoggerFactory))
				{
					(_logger ?? (_logger = Resolve<ILoggerFactory>()?.GetFor(_selfType)))?.Log(_selfType, Level.Warn, $"can't build type of: {typeof(T).NameNice()}", exception);
				}
			}

			return false;
		}

		public void Release()
		{
			foreach(var value in _singletons?.Values ?? new object[] { })
			{
				(value as IDisposable)?.Dispose();
			}

			foreach(var value in _providers?.Values ?? new object[] { })
			{
				(value as IDisposable)?.Dispose();
			}

			_base?.Release();
		}
	}
}