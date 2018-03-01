using System.IO;

namespace Sim.Module.Resources
{
	public interface IResourceFactory
	{
		TResource GetResource<TResource>(ResourceLocator locator);
		Stream GetResourceStream(ResourceLocator locator);
	}
}