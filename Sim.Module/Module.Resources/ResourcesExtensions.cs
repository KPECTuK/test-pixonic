using System;
using System.Linq;

namespace Sim.Module.Resources
{
	internal static class ResourcesExtensions
	{
		internal static bool IsResourceAdapter(this Type source)
		{
			return
				!source.IsGenericType &&
				source
					.GetInterfaces()
					.Any(
						@interface =>
						@interface.IsGenericType &&
						@interface.GetGenericTypeDefinition() == typeof(IResourceAdapter<>));
		}

		internal static Type GetAdapterType(this Type source)
		{
			return source
				.GetInterfaces()
				.Where(
					@interface =>
					@interface.IsGenericType &&
					@interface.GetGenericTypeDefinition() == typeof(IResourceAdapter<>))
				.Select(@interface => @interface.GetGenericArguments().First())
				.FirstOrDefault();
		}
	}
}