using System.Collections.Generic;
using Sim.Module.Data.Config;
using Sim.Module.Data.Ids;
using Sim.Module.Data.State;
using Sim.Render.Realm;
using UnityEngine;

// ReSharper disable once UnusedMember.Global
// ReSharper disable once CheckNamespace
public class RealmComponent : RealmComponentBase
{
	public struct StatusDesc
	{
		public Vector3 Position;
		public string Title;
	}

	public readonly Dictionary<PlayerId, StatusDesc> Statuses = new Dictionary<PlayerId, StatusDesc>();

	protected override void DrawTitle(PlayerState state, HeroData config)
	{
		base.DrawTitle(state, config);

		Statuses[state.Id] = new StatusDesc
		{
			Position = state.Position + Vector3.up * 1.5f,
			Title = $"{state.HeroName}",
		};
	}
}