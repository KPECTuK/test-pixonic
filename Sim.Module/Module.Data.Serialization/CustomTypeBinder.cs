using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace Sim.Module.Data.Serialization
{
	public class CustomTypeBinder : ISerializationBinder
	{
		private readonly Type[] _customTypes;

		public CustomTypeBinder()
		{
			_customTypes = Assembly
				.GetExecutingAssembly()
				.GetTypes()
				.Where(FilterDataContainerTypes)
				.OrderBy(_ => _.Name)
				.Concat(new[]
				{
					typeof(Vector2),
					typeof(Vector3)
				})
				.ToArray();
		}

		public Type BindToType(string assemblyName, string typeName)
		{
			return _customTypes.SingleOrDefault(_ => string.Equals(_.Name, typeName));
		}

		public void BindToName(Type serializedType, out string assemblyName, out string typeName)
		{
			assemblyName = null;
			typeName = serializedType.Name;
		}

		private bool FilterDataContainerTypes(Type type)
		{
			return
				!type.IsAbstract &&
				type.GetInterfaces().Any(_1 => _1.IsGenericType && _1.GetGenericTypeDefinition() == typeof(IDataContainer<>));
		}
	}
}