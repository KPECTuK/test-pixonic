using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Sim.Launcher.NetworkAppender.Filters;

namespace Sim.Launcher.NetworkAppender.Methods
{
	internal abstract class NetworkMethodBase<TChank> : INetworkMethod
	{
		private readonly IEnumerator<TChank> _cursor;
		private readonly INetworkMethodFilter<TChank> _filter;

		protected NetworkMethodBase(IEnumerator<TChank> repositoryCursor, INetworkMethodFilter<TChank> filter)
		{
			_cursor = repositoryCursor;
			_filter = filter;
		}

		protected IEnumerable<IPEndPoint> ResolveTarget(string address, int port)
		{
			return
				Dns
					.GetHostAddresses(address)
					.Select(inspecting => new IPEndPoint(inspecting, port))
					.Where(ep => ep.AddressFamily == AddressFamily.InterNetwork && ep.AddressFamily != AddressFamily.InterNetworkV6)
					.Distinct()
					.ToArray();
		}

		protected byte[] GetBuffer()
		{
			return _cursor.MoveNext() ? _filter.ConvertToBuffer(_cursor.Current) : null;
		}

		public abstract void Commit();
		public abstract void Dispose();
	}
}
