using System;

namespace Sim.Module.Client
{
	public interface ITimeService
	{
		TimeSpan SessionTime { get; }
		TimeSpan SyncInterval { get; }
		TimeSpan CurrentLag { get; }
		TimeSpan AverageLag { get; }

		void StartSession();
		void UpdateCurrentLag(TimeSpan lag, TimeSpan syncInterval);
	}
}