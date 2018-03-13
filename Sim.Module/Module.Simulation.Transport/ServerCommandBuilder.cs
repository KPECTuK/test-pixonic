using System;
using System.Collections.Generic;
using System.Reflection;
using Sim.Module.Command;
using Sim.Module.Generic;

namespace Sim.Module.Simulation.Transport
{
	public class ServerCommandBuilder : CommandBuilderBase
	{
		protected override Dictionary<Type, ConstructorInfo> Correspondences =>
			new Dictionary<Type, ConstructorInfo>
			{
				{ typeof(CommandJoinRequest), typeof(CommandJoinResponse).GetConstructor(Type.EmptyTypes) },          // in
				{ typeof(CommandStateSyncRequest), typeof(CommandStateSyncRequest).GetConstructor(Type.EmptyTypes) }, // out
			};

		public ServerCommandBuilder(IContext context) : base(context) { }
	}
}