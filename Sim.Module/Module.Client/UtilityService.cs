#define USE_NATIVE_RANDOM

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
			#if USE_NATIVE_RANDOM
			return (int)((uint)(UnityEngine.Random.value * uint.MaxValue) - int.MinValue);
			#else
			return _randomGenerator.Next();
			#endif
		}

		public TimeSpan GetInterval()
		{
			return new TimeSpan(GetInt());
		}

		public double GetNormalized()
		{
			#if USE_NATIVE_RANDOM
			return UnityEngine.Random.value;
			#else
			return _randomGenerator.NextDouble();
			#endif
		}

		public HeroData GetHero()
		{
			var heroes = _context.Resolve<IRepository>().GetConfig<HeroData>(null).ToArray();
			return heroes[Math.Abs(GetInt()) % heroes.Length];
		}
	}
}