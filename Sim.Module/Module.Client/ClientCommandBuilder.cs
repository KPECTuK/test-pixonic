using System;
using System.Collections.Generic;
using System.Reflection;
using Sim.Module.Command;
using Sim.Module.Generic;

namespace Sim.Module.Client
{
	public class ClientCommandBuilder : CommandBuilderBase
	{
		protected override Dictionary<Type, ConstructorInfo> Correspondences =>
			new Dictionary<Type, ConstructorInfo>
			{
				{ typeof(CommandStateSyncRequest), typeof(CommandStateSyncResponse).GetConstructor(Type.EmptyTypes) }, // in
				{ typeof(CommandJoinRequest), typeof(CommandJoinRequest).GetConstructor(Type.EmptyTypes) },            // out
			};

		public ClientCommandBuilder(IContext context) : base(context) { }
	}
}