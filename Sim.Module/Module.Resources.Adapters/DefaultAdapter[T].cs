using System.IO;

namespace Sim.Module.Resources.Adapters
{
	public class DefaultAdapter<T> : IResourceAdapter<T>
	{
		public T Deserialize(Stream stream, ResourceLocator locator)
		{
			return default(T);
		}
	}
}