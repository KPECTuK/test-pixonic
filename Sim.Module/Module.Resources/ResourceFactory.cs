using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Sim.Module.Resources
{
	public class ResourceFactory : IResourceFactory
	{
		private readonly Dictionary<Type, ConstructorInfo> _adapters;
		private readonly string _resourcesNamespacePrefix;
		private readonly Assembly _sourceAssembly;

		// TODO: unload assembly event should release reference

		public static IResourceFactory GetFactory(Assembly resourcesAssembly, string resourceRootNamespace)
		{
			// TODO: check namespace path in assembly
			return new ResourceFactory(resourcesAssembly, resourceRootNamespace);
		}

		private ResourceFactory(Assembly resourcesAssembly, string resourceRootNamespace)
		{
			_resourcesNamespacePrefix = string.Intern(resourceRootNamespace);
			_adapters =
				new[] { Assembly.GetExecutingAssembly(), resourcesAssembly }
					.Distinct()
					.SelectMany(assembly => assembly.GetTypes())
					.Where(type => type.IsResourceAdapter())
					.ToDictionary(
						type => type.GetAdapterType(),
						type => type.GetConstructor(Type.EmptyTypes));
			_sourceAssembly = resourcesAssembly;
		}

		private IResourceAdapter<TResource> GetAdapter<TResource>()
		{
			if(!_adapters.ContainsKey(typeof(TResource)))
			{
				throw new InvalidOperationException($"adapter not found for type: {typeof(TResource)}");
			}

			return _adapters[typeof(TResource)].Invoke(null) as IResourceAdapter<TResource>;
		}

		public TResource GetResource<TResource>(ResourceLocator locator)
		{
			using(var stream = GetResourceStream(locator))
			{
				return GetAdapter<TResource>().Deserialize(stream, locator);
			}
		}

		public Stream GetResourceStream(ResourceLocator locator)
		{
			return _sourceAssembly.GetManifestResourceStream($"{_resourcesNamespacePrefix}.{locator.Filename}");
		}
	}
}