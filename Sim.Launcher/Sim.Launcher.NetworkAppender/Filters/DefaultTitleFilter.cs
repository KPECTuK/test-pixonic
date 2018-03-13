using System.Text;

namespace Sim.Launcher.NetworkAppender.Filters
{
	internal class DefaultTitleFilter : INetworkMethodFilter<string>
	{
		public byte[] ConvertToBuffer(string source)
		{
			return Encoding.UTF8.GetBytes(source.Replace("log4unity", "log4net"));
		}
	}
}