using System;
using Sim.Module.Data.Config;

namespace Sim.Module.Client
{
	public interface IRandomService
	{
		int GetInt();
		TimeSpan GetInterval();
		double GetNormalized();
		HeroData GetHero();
	}
}