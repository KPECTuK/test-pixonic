using System;
using Newtonsoft.Json;
using Sim.Module.Data;
using Sim.Module.Data.State;
using Sim.Module.Generic;
using Sim.Module.Resources;
using Sim.Module.Simulator.Data;

namespace Sim.Module.Simulation
{
	public class RepositoryServer : RepositoryBase, IProvider<ServerData>
	{
		private ServerData _serverData;

		public RepositoryServer(IContext context) : base(context) { }

		public override void MergeStates(PlayerState[] difference)
		{
			foreach(var input in difference)
			{
				var stateIndex = Array.FindIndex(CurrentState.PlayerStates, _ => input.Id.Equals(_?.Id));
				if(stateIndex == -1)
				{
					stateIndex = Array.FindIndex(CurrentState.PlayerStates, _ => ReferenceEquals(null, _));
					CurrentState.PlayerStates[stateIndex] = input.Copy();
				}
				else
				{
					CurrentState.PlayerStates[stateIndex].ApplyDifference(input);
				}
			}
		}

		public override void ReloadConfig()
		{
			base.ReloadConfig();

			var resources = Context.Resolve<IResourceFactory>();
			_serverData = JsonConvert
				.DeserializeObject<ServerData>(
					resources.GetResource<string>(new ResourceLocator { Filename = "server.json" }),
					SerializerSettings);
		}

		// IProvider<ServerData>
		ServerData IProvider<ServerData>.Get()
		{
			return _serverData;
		}
	}
}