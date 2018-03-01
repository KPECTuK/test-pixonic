using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Serialization;
using Sim.Module.Data;

namespace Sim.Module.Serialization
{
	public class CustomTypeBinder : ISerializationBinder
	{
		private readonly Type[] _customTypes;

		public CustomTypeBinder()
		{
			_customTypes = Assembly
				.GetExecutingAssembly()
				.GetTypes()
				.Where(FilterDataContzinerTypes)
				.OrderBy(_ => _.Name)
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

		private bool FilterDataContzinerTypes(Type type)
		{
			return
				!type.IsAbstract &&
				type.GetInterfaces().Any(_1 => _1.IsGenericType && _1.GetGenericTypeDefinition() == typeof(IDataContainer<>));
		}
	}
}