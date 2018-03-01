using System.IO;

namespace Sim.Module.Resources
{
	// ReSharper disable once TypeParameterCanBeVariant
	public interface IResourceAdapter<T>
	{
		T Deserialize(Stream stream, ResourceLocator locator);
	}
}