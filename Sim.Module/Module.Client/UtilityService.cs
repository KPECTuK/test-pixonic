using System;
using System.Linq;
using Sim.Module.Data;
using Sim.Module.Data.Config;
using Sim.Module.Generic;

namespace Sim.Module.Client
{
	public class UtilityService : ITimeService, IRandomService
	{
		private readonly IContext _context;
		private readonly Random _randomGenerator = new Random();
		private DateTime _sessionStart;

		public TimeSpan SessionTime => DateTime.UtcNow - _sessionStart;
		public TimeSpan SyncInterval { get; private set; }
		public TimeSpan CurrentLag { get; private set; }
		public TimeSpan AverageLag { get; private set; }

		public UtilityService(IContext context)
		{
			_context = context;
		}

		// time

		public void StartSession()
		{
			_sessionStart = DateTime.UtcNow;
		}

		public void UpdateCurrentLag(TimeSpan lag, TimeSpan syncInterval)
		{
			CurrentLag = lag;
			SyncInterval = syncInterval;
		}

		// random

		public int GetInt()
		{
			return _randomGenerator.Next();
		}

		public TimeSpan GetInterval()
		{
			return new TimeSpan(_randomGenerator.Next());
		}

		public double GetNormalized()
		{
			return _randomGenerator.NextDouble();
		}

		public HeroData GetHero()
		{
			var heroes = _context.Resolve<IRepository>().GetConfig().ToArray();
			return heroes[_randomGenerator.Next() % heroes.Length];
		}
	}
}