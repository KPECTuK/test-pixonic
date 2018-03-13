using System.Text;

namespace Sim.Launcher.NetworkAppender.Filters
{
	internal class DefaultFilter : INetworkMethodFilter<string>
	{
		public byte[] ConvertToBuffer(string source)
		{
			return Encoding.UTF8.GetBytes(source);
		}
	}
}