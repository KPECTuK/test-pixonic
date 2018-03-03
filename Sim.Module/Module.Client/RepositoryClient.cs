using System;
using Sim.Module.Data;
using Sim.Module.Data.State;
using Sim.Module.Generic;

namespace Sim.Module.Client
{
	public class RepositoryClient : RepositoryBase
	{
		public RepositoryClient(IContext context) : base(context) { }

		public override void ReloadState(int frames)
		{
			base.ReloadState(frames);

			var current = CurrentState.PlayerStates;
			var firstFree = Array.FindIndex(current, _ => ReferenceEquals(null, _));
			current[firstFree] = new PlayerState { Id = Context.Resolve<SimService>().LocalId };
			Context.Resolve<IRealmController>().SetupPlayer(current[firstFree]);
		}

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
	}
}