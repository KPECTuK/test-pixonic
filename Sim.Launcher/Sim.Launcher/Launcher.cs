using System;
using System.Threading;
using Sim.Module.Generic;
using Sim.Module.Simulation;

namespace Sim.Launcher
{
	public class Launcher
	{
		private static IContext _context;

		public static void Main(string[] args)
		{
			_context = new ServerCompositionRoot(new LauncherCompositionRoot(null));
			var server = _context.Resolve<SimulatorServer>();
			server.Initialize();
			Console.CancelKeyPress += (sender, eventArgs) => { _context.Release(); };
			while(true)
			{
				server.Update();
				Thread.Sleep(10);
			}
		}
	}
}