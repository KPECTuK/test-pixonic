using System;

namespace Sim.Launcher.NetworkAppender.Methods
{
	internal interface INetworkMethod : IDisposable
	{
		void Commit();
	}
}