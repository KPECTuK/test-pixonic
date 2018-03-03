using System.Collections.Generic;

namespace Sim.Launcher.NetworkAppender.Core
{
	internal interface IRepository<TChank> : IEnumerable<TChank>
	{
		void Enqueue(TChank buffer);
	}
}
