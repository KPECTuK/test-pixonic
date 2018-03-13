using System.IO;
using System.Text;

namespace Sim.Module.Resources.Adapters
{
	public class TextAdapter : IResourceAdapter<string>
	{
		public string Deserialize(Stream stream, ResourceLocator locator)
		{
			return new StreamReader(stream, Encoding.UTF8).ReadToEnd();
		}
	}
}
